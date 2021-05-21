$("#CreateLanguage").click(function (e) {
    e.preventDefault();
    
    var JobSeekerLanguagesViewModel = {
        LanguageLevelId: $("#LanguageLevelId").val(),
        LanguageId: $("#LanguageId").val()
    }

    //Create Skills
    $.ajax({
        type: "post",
        url: "Language/Create",
        data: { JobSeekerLanguagesViewModel },
        success: function (response) {
            //Validate against user adding same skill multiple times
            alert(response.message);
        }
    });

});