
var JobSeekerPersonalDetailsViewModel =
{
    "LastName": "",
    "FirstName": "",
    "Phone": "",
    "StreetNumber": "",
    "StreetName": "",
    "BuildingNumber": "",
    "PersonalProfile": "",
    "GenderId": "",
    "EthnicityId": "",
    "CityId": "",
    "ProvinceId": "",
    "TitleId": "",
    "AddressId": "",
    "JobSeekerId": "",
    "IdNumber": "",
    "GenderId": ""
}

$(document).ready(function () {
    //Initialize them with preselected options
    JobSeekerPersonalDetailsViewModel.GenderId = $("select#GenderId").children("option:selected").val();
    JobSeekerPersonalDetailsViewModel.TitleId = $("select#TitleId").children("option:selected").val();
    JobSeekerPersonalDetailsViewModel.EthnicityId = $("select#EthnicityId").children("option:selected").val();
    JobSeekerPersonalDetailsViewModel.CityId = $("select#CityId").children("option:selected").val();
    JobSeekerPersonalDetailsViewModel.ProvinceId = $("select#ProvinceId").children("option:selected").val();
    //Get Gender
    $("select#GenderId").change(function () {
        JobSeekerPersonalDetailsViewModel.GenderId = $(this).children("option:selected").val();
    });

    //Get Title
    $("select#TitleId").change(function () {
        JobSeekerPersonalDetailsViewModel.TitleId = $(this).children("option:selected").val();
    });

    //Get Ethnicity
    $("select#EthnicityId").change(function () {
        JobSeekerPersonalDetailsViewModel.EthnicityId = $(this).children("option:selected").val();
    });

    //Get City
    $("select#CityId").change(function () {
        JobSeekerPersonalDetailsViewModel.CityId = $(this).children("option:selected").val();
    });

    //Get City
    $("select#ProvinceId").change(function () {
        JobSeekerPersonalDetailsViewModel.ProvinceId = $(this).children("option:selected").val();
    });

    //Get City
    $("select#ProvinceId").change(function () {
        JobSeekerPersonalDetailsViewModel.ProvinceId = $(this).children("option:selected").val();
    });

    //Get the state of the checkbox and hide dates based on the state
    $("#IsCurrentCompany").click(function () {

        var StartDate = $("#StartDate");
        var EndDate = $("#EndDate");

        if (this.checked) {
            StartDate.prop("disabled", true);
            EndDate.prop("disabled", true);
            EndDate.prop("required", false);
            StartDate.prop("required", false);
            //alert('Checked');
        }
        else {
            StartDate.prop("disabled", false);
            EndDate.prop("disabled", false);
            EndDate.prop("required", true);
            StartDate.prop("required", true);
        }
    });


});

$("#UpdatePersonalDetails").click(function () {
    //Get Personal Details Data
    //Should have assigned them straight to the object
    var FirstName = $("#FirstName").val();
    var LastName = $("#LastName").val();
    var IdNumber = $("#IdNumber").val();
    var Phone = $("#Phone").val();
    var StreetNumber = $("#StreetNumber").val();
    var StreetName = $("#StreetName").val();
    var BuildingNumber = $("#BuildingNumber").val();
    var PersonalProfile = $("#PersonalProfile").val();
    var AddreeesId = $("#AddressId").val();

    JobSeekerPersonalDetailsViewModel.FirstName = FirstName;
    JobSeekerPersonalDetailsViewModel.LastName = LastName;
    JobSeekerPersonalDetailsViewModel.IdNumber = IdNumber;
    JobSeekerPersonalDetailsViewModel.Phone = Phone;
    JobSeekerPersonalDetailsViewModel.StreetName = StreetName;
    JobSeekerPersonalDetailsViewModel.StreetNumber = StreetNumber;
    JobSeekerPersonalDetailsViewModel.BuildingNumber = BuildingNumber;
    JobSeekerPersonalDetailsViewModel.PersonalProfile = PersonalProfile;
    JobSeekerPersonalDetailsViewModel.AddressId = AddreeesId;
    //Work on Selects
    //Update Personal Details
    $.ajax({
        type: "post",
        url: "UpdatePersonalDetails",
        data: { JobSeekerPersonalDetailsViewModel },
        success: function (response) {
            console.log(response);
            //Here we can redirect to success page as well, whatever works with time remaining.
            if (response.status === "ok") {
                $("#UpdatePersonalDetails").html("Update");
                alert(response.messages);
            } else {
                //Show error message through alert or on html page
                alert(response.messages);
            }
        }
    });

});

//Update Education
$(document).on('click', '.EachEducation', function () {
    //Get Right Experience

    var id = $(this).closest('div').attr("id");
    id = id.substr(13);

    var JobSeekerEducationViewModel =
    {
        GraduationYear: $("#EducationGraduationYear_" + id).val(),
        Institution: $("#EducationInstitution_" + id).val(),
        QualificationId: $("#EducationQualificationId_" + id).val(),
        FieldOfStudy: $("#EducationFieldOfStudy_" + id).val(),
        EducationId: id,
        
    }
    //console.log(JobSeekerExperienceViewModel);

    $.ajax({
        type: "post",
        url: 'UpdateEducation',
        data: { JobSeekerEducationViewModel },
        success: function (response) {
            console.log(response);
            //Here we can redirect to success page as well, whatever works with time remaining.
            if (response.status == "ok") {
                alert(response.messages);
            } else {
                alert("Not Updated");
            }
        }
    });

});

//Update Experience
$(document).on('click', '.EachExperience', function () {
    //Get Right Experience

    var id = $(this).closest('div').attr("id");
    id = id.substr(20);

    var JobSeekerExperienceViewModel =
    {
        JobTitle : $("#ExperienceJobTitle_" + id).val(),
        CompanyName : $("#ExperienceCompanyName_" + id).val(),
        JobCategory : $("#ExperienceJobTitle_" + id).val(),
        IsCurrentCompany: $("#WorkExperienceIsCurrentCompany_" + id).val(),
        JobCategoryId: $("#ExperienceCategory_" + id).val(),
        StartDate : $("#ExperienceStartDate_" + id).val(),
        EndDate : $("#ExperienceEndDate_" + id).val(),
        WorkExperienceId : id
    }
    //console.log(JobSeekerExperienceViewModel);
    //Update Personal Details
   
    $.ajax({
        type: "post",
        url: "UpdateExperience",
        data: { JobSeekerExperienceViewModel },
        success: function (response) {
            console.log(response);
            //Here we can redirect to success page as well, whatever works with time remaining.
            if (response.status == "ok") {
                alert(response.messages);
            } else {
                alert("Something went wrong, Report to System Adminstrator");
            }
        }
    });

});

//Update Skills
$(document).on('click', '.EachSkill', function (e) {
    //Get Right Experience
    e.preventDefault();
    var id = $(this).closest('div').attr("id");
    id = id.substr(9);

    var JobSeekerSkillsViewModel =
    {
        SkillId: id,
        SkillLevelId: $("#SkillLevel_" + id).val()

    }
    console.log(JobSeekerSkillsViewModel);
   
    $.ajax({
        type: "post",
        url: 'UpdateSkill',
        data: { JobSeekerSkillsViewModel },
        success: function (response) {
            console.log(response);
            //Here we can redirect to success page as well, whatever works with time remaining.
            if (response.status == "ok") {
                alert(response.messages);
            } else {
                alert("Not Updated");
            }
        }
    });

});

//Update Languge
$(document).on('click', '.EachLanguage', function () {
    //Get Right Experience

    var id = $(this).closest('div').attr("id");
    id = id.substr(11);

    var JobSeekerLanguagesViewModel =
    {
        LanguageId: id,
        LanguageLevelId: $("#LangugeLevel_" + id).val()

    }
    console.log(JobSeekerLanguagesViewModel);
  
    $.ajax({
        type: "post",
        url: 'UpdateLanguage',
        data: { JobSeekerLanguagesViewModel },
        success: function (response) {
            console.log(response);
            //Here we can redirect to success page as well, whatever works with time remaining.
            if (response.status == "ok") {
                alert(response.messages);
            } else {
                alert("Not Updated");
            }
        }
    });

});

$(document).ready(function (e) {

    e.preventDefault();

    $("#frmUploadCV").on('submit', (function (e) {

        $.ajax({
            type: "post",
            url: "UploadCV",
            data: new FormData(this),
            contentType: false,
            cache: false,
            processData: false,
            success: function (response) {
                console.log(response);
            }
        });

    }));
});

$(document).ready(function (e) {

    e.preventDefault();

    $("#frmUploadAcademicRecord").on('submit', (function (e) {

        $.ajax({
            type: "post",
            url: "UploadAcademicRecord",
            data: new FormData(this),
            contentType: false,
            cache: false,
            processData: false,
            success: function (response) {
                console.log(response);
            }
        });

    }));
});

$(document).ready(function (e) {

    e.preventDefault();

    $("#frmUploadID").on('submit', (function (e) {

        $.ajax({
            type: "post",
            url: "UploadId",
            data: new FormData(this),
            contentType: false,
            cache: false,
            processData: false,
            success: function (response) {
                console.log(response);
            }
        });

    }));
});