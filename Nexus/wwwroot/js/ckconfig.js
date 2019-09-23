CKEDITOR.editorConfig = function (config) {
    config.plugins =
        'dialogui,dialog,about,a11yhelp,dialogadvtab,basicstyles,bidi,blockquote,' +
        'notification,button,toolbar,clipboard,panelbutton,panel,floatpanel,colorbutton,' +
        'colordialog,templates,menu,contextmenu,copyformatting,div,resize,elementspath,' +
        'enterkey,entities,popup,filetools,filebrowser,find,fakeobjects,flash,' +
        'floatingspace,listblock,richcombo,font,forms,format,horizontalrule,' +
        'htmlwriter,iframe,wysiwygarea,image,indent,indentblock,indentlist,' +
        'smiley,justify,menubutton,language,link,list,liststyle,magicline,maximize,' +
        'newpage,pagebreak,pastetext,pastefromword,preview,print,removeformat,save,' +
        'selectall,showblocks,showborders,sourcearea,specialchar,scayt,stylescombo,' +
        'tab,table,tabletools,tableselection,undo,lineutils,widgetselection,widget,' +
        'notificationaggregator,uploadwidget,uploadimage,wsc';
    config.skin = 'moono-lisa';

    config.toolbarGroups = [
        { name: 'document', groups: ['mode', 'document', 'doctools'] },
        { name: 'clipboard', groups: ['clipboard', 'undo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
        { name: 'forms', groups: ['forms'] },
        '/',
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
        { name: 'links', groups: ['links'] },
        { name: 'insert', groups: ['insert'] },
        '/',
        { name: 'styles', groups: ['styles'] },
        { name: 'colors', groups: ['colors'] },
        { name: 'tools', groups: ['tools'] },
        { name: 'others', groups: ['others'] },
        { name: 'about', groups: ['about'] }
    ];

    //config.codeSnippet_languages = {
    //    csharp: 'C#',
    //    c: 'C',
    //    cpp: 'C++',
    //    go: 'Go',
    //    javascript: 'JavaScript',
    //    python: 'Python'
    //};

    config.csharp_codeClass = 'language-csharp';

    config.removeButtons = 'HiddenField,ImageButton,Button,Select,Textarea,TextField,Radio,Checkbox,Form';
};