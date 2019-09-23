using System;
using System.IO;
using System.Linq;
using AutoMapper;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;

namespace Nexus.Profiles
{
    public class BookViewModelProfile : Profile
    {
        public BookViewModelProfile()
        {
            CreateMap<BookDto, BookViewModel>()
                .ForMember(vm => vm.CategoryId, options => options.NullSubstitute(0))
                .ForMember(vm => vm.Id, config => config.MapFrom(source => source.Id))
                .ForMember(vm => vm.CoverImageFile, config => config.Ignore())
                .ForMember(vm => vm.ReadingStatusSelections, config => config.Ignore())
                .ForMember(vm => vm.Categories, options => options.Ignore())
                .AfterMap((dto, vm) =>
                {
                    vm.Categories = dto.BookCategories.Select(bc => bc.CategoryId).ToArray();
                });

            CreateMap<BookViewModel, BookDto>()
                .ForMember(dto => dto.CategoryId, options => options.Condition(vm => vm.CategoryId.HasValue && vm.CategoryId != 0))
                .ForMember(dto => dto.BookCategories, options => options.Ignore())
                .ForMember(sourceDto => sourceDto.CoverImage, o => o.MapFrom((vm, dto) =>
                {
                    // Edit scenario
                    if (vm.Id != 0)
                    {
                        if (vm.CoverImageFile != null && vm.CoverImageFile.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                vm.CoverImageFile.CopyTo(memoryStream);
                                return memoryStream.ToArray();
                            }
                        }

                        // eğer yeni bir resim seçmemişsem vm den dto ya geçerken dto'nun mevcut resmini tekrar vm ye set ediyorum
                        // diğer bir deyişle eğer yeni resim set edilmemişse varolan resim kullanılıyor
                        // yoksa yeni bir resim gelmediği için mevcut resim de giderdi
                        return dto.CoverImage; // by-pass
                    }
                    else
                    {
                        if (vm.CoverImageFile != null && vm.CoverImageFile.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                vm.CoverImageFile.CopyTo(memoryStream);
                                return memoryStream.ToArray();
                            }
                        }
                        // yeni kitap eklenirken resim eklenmemiş, o zaman bir şey yapma: null.
                        return null;
                    }
                }))
                .ForMember(sourceDto => sourceDto.CoverImageMime, o => o.MapFrom((vm, dto) =>
                {
                    if (vm.Id != 0)
                    {
                        if (vm.CoverImageFile != null && vm.CoverImageFile.Length > 0)
                        {
                            string mime = vm.CoverImageFile.ContentType;
                            return mime;
                        }
                        return dto.CoverImageMime;
                    }
                    else
                    {
                        if (vm.CoverImageFile != null && vm.CoverImageFile.Length > 0)
                        {
                            string mime = vm.CoverImageFile.ContentType;
                            return mime;
                        }
                        return default(string);
                    }
                }))
                .AfterMap((vm, dto) =>
                {
                    dto.BookCategories = vm.Categories.Select(id => new BookCategoryDto { CategoryId = id }).ToList();
                });
        }
    }
}