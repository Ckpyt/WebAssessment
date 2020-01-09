<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="WebAssessment.contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         <div class="hero-banner--sm__content">
        <h1>Contact me</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Contact me</li>
          </ol>
        </nav>
      </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
  <!-- ================ contact section start ================= -->
  <section class="section-margin">
    <div class="container">

      <div class="row">
        <div class="col-md-4 col-lg-3 mb-4 mb-md-0">
          
          <div class="media contact-info">
            <span class="contact-info__icon"><i class="ti-headphone"></i></span>
            <div class="media-body">
              <h3><a href="tel:454545654">+64 (027) 503 6490</a></h3>
              <p>Mon to Fri 9am to 6pm</p>
            </div>
          </div>
          <div class="media contact-info">
            <span class="contact-info__icon"><i class="ti-email"></i></span>
            <div class="media-body">
              <h3><a href="mailto:ckpyt.site@gmail.com">ckpyt.site@gmail.com</a></h3>
              <p>Send me your query anytime!</p>
            </div>
          </div>
        </div>
        <div class="col-md-8 col-lg-9">
          
            <div class="row">
              <div class="col-lg-5">
                <div class="form-group">
                  <asp:TextBox runat="server" class="form-control" name="name" id="name" type="text" placeholder="Enter your name"/>
                </div>
                <div class="form-group">
                  <asp:TextBox runat="server" class="form-control" name="email" id="email" type="email" placeholder="Enter email address"/>
                </div>
                <div class="form-group">
                  <asp:TextBox runat="server"  class="form-control" name="subject" id="subject" type="text" placeholder="Enter Subject"/>
                </div>
              </div>
              <div class="col-lg-7">
                <div class="form-group">
                    <asp:TextBox runat="server" TextMode="multiline" class="form-control different-control w-100" name="message" id="message" cols="30" rows="5" placeholder="Enter Message"></asp:TextBox>
                </div>
              </div>
            </div>
            <div class="form-group text-center text-md-right mt-3">
              <asp:button class="button button-contactForm" runat="server" asp:text="Send Message" Text="Send Message" OnClick="SendMessage"/>
            </div>
          
        </div>
      </div>
    </div>
  </section>
	<!-- ================ contact section end ================= -->


</asp:Content>
