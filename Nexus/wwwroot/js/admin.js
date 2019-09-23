function display_grid_errors(e) {
    if (e.errors) {
        var message = "One or more errors occurred:\n";

        // Create a message containing all errors.
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });

        // Display the message.
        alert(message);

        // Cancel the changes.
        var grid = $("#grid").data("kendoGrid");
        grid.cancelChanges();
    }
}
