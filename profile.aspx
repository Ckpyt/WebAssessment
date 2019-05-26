<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="WebAssessment.profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    
  <!--================ Hero sm Banner start =================-->      
    

      <div class="hero-banner--sm__content">
        <h1>Your profile</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Profile</li>
          </ol>
        </nav>
      </div>

  <!--================ Hero sm Banner end =================-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <script>

      function ChangeDetailsBtnClc() {
          alert(1);
          namestr = document.getElementById('<%= Login.ClientID %>').innerText.length;
          oldPass = document.getElementById('<%= Password.ClientID %>').value.length;
          passstr = document.getElementById('<%= newPassword.ClientID %>').value.length;
          pass = document.getElementById('<%= newPassword.ClientID %>').value;
          conf = document.getElementById('<%= Confirm.ClientID %>').value;
          txtMail = document.getElementById('<%= UserEmail.ClientID %>').value;
          alert(2);
          let k = 0;
          if (namestr == '') {
              document.getElementById("invMsgLogin2").className = "alert alert-danger";
              document.getElementById("invMsgLogin2").innerHTML = "name should contain at least 1 symbol";
          } else {
              k++;
              document.getElementById("invMsgLogin2").className = "invisible";
          }

          if (oldPass > 0 && passstr == 0) {
              alert(3);
              document.getElementById("invMsgPass2").className = "alert alert-danger";
              document.getElementById("invMsgPass2").innerHTML = "password should contain at least 1 symbol";
          } else {
              document.getElementById("invMsgPass2").className = "invisible";
              k++;
          }

          alert(4 + " " + oldPass + " " + passstr);
          if (passstr > 0 && oldPass == 0) {
              alert(5);
              document.getElementById("invMsgPass1").className = "alert alert-danger";
              document.getElementById("invMsgPass1").innerHTML = "please, enter your old password";
          } else {
              k++;
              document.getElementById("invMsgPass1").className = "invisible";
          }

          if (pass != conf) {
              document.getElementById("invMsg2").className = "alert alert-danger";
              document.getElementById("invMsg2").innerHTML = "password and confirm password do not match";
              return false;
          } else {
              k++;
              document.getElementById("invMsg2").className = "invisible";
          }

          if (txtMail.length == '0') {

              document.getElementById("invMsgEmail2").className = "alert alert-danger";
              document.getElementById("invMsgEmail2").innerHTML = "email should not be empty";
              return false;
          } else {
              if (!validateEmail(txtMail)) {
                  document.getElementById("invMsgEmail2").className = "alert alert-danger";
                  document.getElementById("invMsgEmail2").innerHTML = "sorry, your email is not correct";
                  return false;
              } else {
                  document.getElementById("invMsgEmail2").className = "invisible";
              }
          }
          alert(10);
          return k == 4;
      }

      function DeleteConfirm() {
          return confirm("Do you want delete your account?");
      }
  </script>
  <!--================ Pricing section start =================-->      
  <section class="section-margin">
    <div class="container">
      <div class="section-intro pb-85px text-center">
        Here you can change your profile
      </div>

      <div class="row">

        
        <div class="text-center centered-element">
            <!-- START POPUP BLOCK -->
            <table >
                <tr>
                    <td>
                        <div class="nav-link tbl"> Login: </div>
                    </td>
                    <td>
                        <asp:Label id="Login" runat="server" >looggooo</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"> 
                        <div id="invMsgLogin2" class="invisible">message</div>
                    </td>
                <tr>
                    <td>
                        <div class="nav-link tbl">Old password: </div>
                    </td>
                    <td>
                        <asp:TextBox id="Password" runat="server" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"> 
                        <div id="invMsgPass1" class="invisible">message</div>
                    </td>
                <tr>
                    <td>
                        <div class="nav-link tbl"> New password: </div>
                    </td>
                    <td>
                        <asp:TextBox id="newPassword" runat="server" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"> 
                        <div id="invMsgPass2" class="invisible">message</div>
                    </td>
                <tr>
                    <td>
                        <div class="nav-link tbl"> Confirm password: </div>
                    </td>
                    <td>
                        <asp:TextBox id="Confirm" runat="server" TextMode="Password" />
                    </td>

                </tr>
                <tr>
                    <td colspan="2"> 
                        <div id="invMsg2" class="invisible">message</div>
                    </td>
                <tr>
                    <td>
                        <div class="nav-link tbl"> E-mail: </div>
                    </td>
                    <td>
                        <asp:TextBox id="UserEmail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"> 
                        <div id="invMsgEmail2" class="invisible">message</div>
                    </td>
                </table>
            <!-- END POPUP BLOCK -->
            <!-- START POPUP BLOCK -->
            <br />
            <div class="popblock popblocksubbtn">
                <asp:Button ID="RemoveUser" class="button blue-btn  ibvm" runat="server" Text="Delete account" OnClick="DeleteAccount_Click"/>
                <asp:Button ID="ChangeDetails" class="button blue-btn  ibvm" runat="server" Text="Change profile" OnClick="Profilelnk_Click"/>
            </div>
            <!-- END POPUP BLOCK -->
        </div>

      </div>
    </div>
  </section>
  <!--================ Pricing section end =================--> 
  



  <!--================ Testimonial section start =================-->      
  <section class="section-padding bg-magnolia">
    <div class="container">
      <div class="section-intro pb-5 text-center">
        <h2 class="section-intro__title">Client Says Me</h2>
        <p class="section-intro__subtitle">Vel aliquam quis, nulla pede mi commodo tristique nam hac. Luctus torquent velit felis commodo pellentesque nulla cras. Tincidunt hacvel alivquam </p>
      </div>

      <div class="owl-carousel owl-theme testimonial">
        <div class="testimonial__item text-center">
          <div class="testimonial__img">
            <img src="img/testimonial/testimonial1.png" alt="">
          </div>
          <div class="testimonial__content">
            <h3>Stephen Mcmilan</h3>
            <p>Executive, ACI Group</p>
            <p class="testimonial__i">Also made from. Give may saying meat there from heaven it lights face had is gathered god earth light for life may itself shall whales made they're blessed whales also made from give may saying meat. There from heaven it lights face had also made from. Give may saying meat there from heaven</p>
          </div>
        </div>
        <div class="testimonial__item text-center">
          <div class="testimonial__img">
            <img src="img/testimonial/testimonial1.png" alt="">
          </div>
          <div class="testimonial__content">
            <h3>Stephen Mcmilan</h3>
            <p>Executive, ACI Group</p>
            <p class="testimonial__i">Also made from. Give may saying meat there from heaven it lights face had is gathered god earth light for life may itself shall whales made they're blessed whales also made from give may saying meat. There from heaven it lights face had also made from. Give may saying meat there from heaven</p>
          </div>
        </div>
        <div class="testimonial__item text-center">
          <div class="testimonial__img">
            <img src="img/testimonial/testimonial1.png" alt="">
          </div>
          <div class="testimonial__content">
            <h3>Stephen Mcmilan</h3>
            <p>Executive, ACI Group</p>
            <p class="testimonial__i">Also made from. Give may saying meat there from heaven it lights face had is gathered god earth light for life may itself shall whales made they're blessed whales also made from give may saying meat. There from heaven it lights face had also made from. Give may saying meat there from heaven</p>
          </div>
        </div>
      </div>
    </div>
  </section>
  <!--================ Testimonial section end =================-->      


  <!--================ Start Clients Logo Area =================-->
	<section class="clients_logo_area section-padding">
		<div class="container">
			<div class="clients_slider owl-carousel">
				<div class="item">
					<img src="img/clients-logo/c-logo-1.png" alt="">
				</div>
				<div class="item">
					<img src="img/clients-logo/c-logo-2.png" alt="">
				</div>
				<div class="item">
					<img src="img/clients-logo/c-logo-3.png" alt="">
				</div>
				<div class="item">
					<img src="img/clients-logo/c-logo-4.png" alt="">
				</div>
				<div class="item">
					<img src="img/clients-logo/c-logo-5.png" alt="">
				</div>
			</div>
		</div>
	</section>
	<!--================ End Clients Logo Area =================-->


</asp:Content>
