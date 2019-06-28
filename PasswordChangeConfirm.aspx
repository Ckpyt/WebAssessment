<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PasswordChangeConfirm.aspx.cs" Inherits="WebAssessment.PasswordChangeConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
          <div class="hero-banner--sm__content">
        <h1>Password changing confirmation</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Password confirm</li>
          </ol>
        </nav>
      </div>

</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section class="section-margin">
         <div class="container text-center centered-element">
             <asp:Label ID="PassCCinvMsg" runat="server" class="invisible"></asp:Label>
         </div>
    <div class="container text-center centered-element">
        <div class="section-intro pb-85px text-center">
        Please, type your confirmation code(ctrl+c, ctrl+v avaliable)
        </div>
        <div class="container text-center centered-element">
            <table class="text-center centered-element">
                <tr>
                    <td>
                        <asp:TextBox ID="ConfirmBox" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:button class="button blue-btn  ibvm" ID="ConfirmButton" runat="server" text="Change password" OnClick="ConfirmButton_ClickAsync" />
                    </td>
                </tr>
            </table>
            
        </div>
    </div>
</section>
</asp:Content>
