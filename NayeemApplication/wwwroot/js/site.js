// Change Password Validation

"use strict"; var KTSignupGeneral = function () { var e, t, a, s, r = function () { return 100 === s.getScore() }; return { init: function () { e = document.querySelector("#frmChangePassword"), t = document.querySelector("btnSubmit"), s = KTPasswordMeter.getInstance(e.querySelector('[data-kt-password-meter="true"]')), a = FormValidation.formValidation(e, { fields: { "OldPassword": { validators: { notEmpty: { message: "Old Password is required" } } }, Password: { validators: { notEmpty: { message: "The password is required" }, callback: { message: "Please enter valid password", callback: function (e) { if (e.value.length > 0) return r() } } } }, "ConfirmPassword": { validators: { notEmpty: { message: "The password confirmation is required" }, identical: { compare: function () { return e.querySelector('[name="Password"]').value }, message: "The password and its confirm are not the same" } } } }, plugins: { trigger: new FormValidation.plugins.Trigger({ event: { Password: !1 } }), bootstrap: new FormValidation.plugins.Bootstrap5({ rowSelector: ".fv-row", eleInvalidClass: "", eleValidClass: "" }) } }), e.querySelector('input[name="Password"]').addEventListener("input", (function () { this.value.length > 0 && a.updateFieldStatus("Password", "NotValidated") })) } } }(); KTUtil.onDOMContentLoaded((function () { KTSignupGeneral.init() }));

//Change Password Submit

$('#btnSubmit').click(function () {
    $('#frmChangePassword').on('submit', function (event) {
        event.preventDefault();
        var frmdata = $('#frmChangePassword').serialize();
        console.log(frmdata);
        Swal.fire({
            title: 'Are you sure?',
            text: "You want to save this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, save it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/Auth/Account/ChangePassword",
                    type: "POST",
                    data: frmdata,
                    dataType: "json",
                    beforeSend: function () {
                        $('#btnSubmit').attr('disabled', 'disabled');
                        $('#btnSubmit').val('Submitting...');
                    },
                }).done(function (data) {
                    $("#btnSubmit").removeAttr('disabled');
                    $("#ChangePasswordModal").modal('hide');
                    $("#frmChangePassword").get(0).reset();
                    if (data) {
                      
                        swal.fire('success', 'Updated Successfully!', 'success').then(function () {
                            location.reload();
                        });
                    } else {
                        swal.fire('warning', 'Failed!', 'warning');
                    }
                   
                  
                }).fail(function () {
                    $("#frmChangePassword").get(0).reset();
                    $("#ChangePasswordModal").modal('hide');
                    $("#btnSubmit").removeAttr('disabled');
                    swal.fire('warning', 'Failed!', 'warning');
                })
            }
        });
    })
})



