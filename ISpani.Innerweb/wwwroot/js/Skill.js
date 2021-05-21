$("#CreateSkill").click(function (e) {
    e.preventDefault();

    var JobSeekerSkillsViewModel = {
        SkillLevelId: $("#SkillLeveId").val(),
        SkillId: $("#SkillId").val()
    }
   
    //Create Skills
    $.ajax({
        type: "post",
        url: "Skills/Create",
        data: { JobSeekerSkillsViewModel },
        success: function (response) {
            //Validate against user adding same skill multiple times

            alert(response.message);
        }
    });

});