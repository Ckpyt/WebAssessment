function DisplayModal() {
    document.getElementById("overlay-").style.height = document.body.clientHeight + 'px';
    document.getElementById("overlay-").className = "OverlayEffect";
    document.getElementById("modalMsg-1").className = "ShowModal";
    document.getElementById("modalMsg-2").className = "ShowModal";
    document.getElementById("SignBtn").className = "HideModal";
    document.getElementById("header").className = "";
    return false;
}

function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}



function RegisterCheck(namestr, passstr, pass, conf, txtMail) {

    let k = 0;
    if (namestr == '') {
        document.getElementById("invMsgLogin").className = "invBox alert alert-danger";
        document.getElementById("invMsgLogin").innerHTML = "name should contain at least 1 symbol";
    } else {
        k++
        document.getElementById("invMsgLogin").className = "invisible";
    }

    if (passstr == '') {
        document.getElementById("invMsgPass").className = "invBox alert alert-danger";
        document.getElementById("invMsgPass").innerHTML = "password should contain at least 1 symbol";
    } else {
        k++;
        document.getElementById("invMsgPass").className = "invisible";
    }

    if (pass != conf) {
        document.getElementById("invMsg").className = "invBox alert alert-danger";
        document.getElementById("invMsg").innerHTML = "password and confirm password do not match";

    } else {
        k++;
        document.getElementById("invMsg").className = "invisible";
    }

    if (txtMail.length == '0') {

        document.getElementById("invMsgEmail").className = "invBox alert alert-danger";
        document.getElementById("invMsgEmail").innerHTML = "email should not be empty";
        return false;
    } else {
        if (!validateEmail(txtMail)) {
            document.getElementById("invMsgEmail").className = "invBox alert alert-danger";
            document.getElementById("invMsgEmail").innerHTML = "sorry, your email is not correct";
            return false;
        } else {
            document.getElementById("invMsgEmail").className = "invisible";
        }
    }
    return k == 3;
}

$(function () {
  "use strict";

  var nav_offset_top = $('header').height() + 50; 
    /*-------------------------------------------------------------------------------
	  Navbar 
	-------------------------------------------------------------------------------*/
    function navbarFixed(){
        if ( $('.header_area').length ){ 
            $(window).scroll(function() {
                var scroll = $(window).scrollTop();   
                if (scroll >= nav_offset_top ) {
                    $(".header_area").addClass("navbar_fixed");
                } else {
                    $(".header_area").removeClass("navbar_fixed");
                }
            });
        };
    };
    navbarFixed();


    /*-------------------------------------------------------------------------------
	  clients logo slider
	-------------------------------------------------------------------------------*/
    if ($('.clients_slider').length) {
      $('.clients_slider').owlCarousel({
          loop: true,
          margin: 30,
          items: 5,
          nav: false,
          dots: false,
          responsiveClass: true,
          autoplay: 2500,
          slideSpeed: 300,
          paginationSpeed: 500,
          responsive: {
              0: {
                  items: 1,
              },
              768: {
                  items: 3,
              },
              1024: {
                  items: 4,
              },
              1224: {
                  items: 5
              }
          }
      })
    }


    /*-------------------------------------------------------------------------------
	  testimonial slider
	-------------------------------------------------------------------------------*/
    if ($('.testimonial').length) {
      $('.testimonial').owlCarousel({
          loop: true,
          margin: 30,
          items: 5,
          nav: false,
          dots: true,
          responsiveClass: true,
          slideSpeed: 300,
          paginationSpeed: 500,
          responsive: {
              0: {
                  items: 1
              }
          }
      })
    }


  /*-------------------------------------------------------------------------------
	  Mailchimp 
	-------------------------------------------------------------------------------*/
	function mailChimp() {
		$('#mc_embed_signup').find('form').ajaxChimp();
	}
  mailChimp();
  
});


