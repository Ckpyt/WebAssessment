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
          
          namestr = document.getElementById('<%= Login.ClientID %>').innerText.length;
          oldPass = document.getElementById('<%= Password.ClientID %>').value.length;
          passstr = document.getElementById('<%= newPassword.ClientID %>').value.length;
          pass = document.getElementById('<%= newPassword.ClientID %>').value;
          conf = document.getElementById('<%= Confirm.ClientID %>').value;
          txtMail = document.getElementById('<%= UserEmail.ClientID %>').value;
          
          let k = 0;
          if (namestr == '') {
              document.getElementById("invMsgLogin2").className = "alert alert-danger";
              document.getElementById("invMsgLogin2").innerHTML = "name should contain at least 1 symbol";
          } else {
              k++;
              document.getElementById("invMsgLogin2").className = "invisible";
          }

          if (oldPass > 0 && passstr == 0) {
              
              document.getElementById("invMsgPass2").className = "alert alert-danger";
              document.getElementById("invMsgPass2").innerHTML = "password should contain at least 1 symbol";
          } else {
              document.getElementById("invMsgPass2").className = "invisible";
              k++;
          }

          
          if (passstr > 0 && oldPass == 0) {
              
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
                        <asp:Label id="invMsgEmail2" runat="server" class="invisible">message</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="nav-link tbl"> Your description: </div>
                    </td>
                    <td>
                        <asp:TextBox id="Description" runat="server" Height="132px" TextMode="MultiLine" Width="229px" />
                    </td>
                </tr>
                </table>
            <!-- END POPUP BLOCK -->
            <!-- START POPUP BLOCK -->
            <br />
            <div class="popblock popblocksubbtn">
                <asp:Button ID="AdminMode" class="button blue-btn  ibvm" runat="server" Text="Open admin page" OnClick="AdminMode_Click" Visible="False" />
                <asp:Button ID="RemoveUser" class="button blue-btn  ibvm" runat="server" Text="Delete account" OnClick="DeleteAccount_Click"/>
                <asp:Button ID="ChangeDetails" class="button blue-btn  ibvm" runat="server" Text="Change profile" OnClick="Profilelnk_Click"/>
            </div>
            <!-- END POPUP BLOCK -->
        </div>

      </div>
    </div>
  </section>
  <!--================ Pricing section end =================--> 
</asp:Content>
