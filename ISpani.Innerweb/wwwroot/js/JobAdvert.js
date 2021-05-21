$("#Apply").click(function (e) {
    e.preventDefault();
    //alert('Apply');
    var JobSeekerJobDetailsViewModel = {
        JobAdvertId: $("#JobAdvertId").val(),
        RecruiterId: $("#RecruiterId").val(),
        CompanyId: $("#CompanyId").val()
    }
    console.log(JobSeekerJobDetailsViewModel);
    //Create Skills
    $.ajax({
        type: "post",
        url: "ApplyForJob",
        data: { JobSeekerJobDetailsViewModel },
        cache:false,
        success: function (response) {
            //Validate against user adding same skill multiple times
            alert('Apply');
            console.log(JobSeekerJobDetailsViewModel);
        }
    });

});

/*$("#Interview").click(function (e) {
    e.preventDefault();
    alert('Interview');

    $.ajax({
        type: "post",
        url: "InviteForInterView",
        data: { "jobAdvertId": $("#AdvertIdInterview").val(), "jobSeekerId": $("#SeekerIdInterview").val()},
        cache:false,
        success: function (response) {
            //Validate against user adding same skill multiple times
            alert(response.message);
        }
    });

});*/

