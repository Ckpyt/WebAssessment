﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GetSaveNamesApi.aspx.cs" Inherits="WebAssessment.Games.ColonyRuler.GetSaveNamesApi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
            <div class="hero-banner--sm__content">
        <h1>Colony ruler</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active">Colony ruler</li>
          </ol>
            <nav class="navbar navbar-expand-lg navbar-light">
                Colony Ruler Api:
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <div class="collapse navbar-collapse offset" id="navbarSupportedContent">
                    <ul class="nav ">
                        <li class="nav-item"><a class="nav-link" href='<%= ResolveUrl("~/Games/ColonyRuler/GetLanguagesListApi.aspx")%>'>Get languages list</a></li>
                        <li class="nav-item"><a class="nav-link" href='<%= ResolveUrl("~/Games/ColonyRuler/GetLocalizationApi.aspx")%>'>Get localization</a></li>
                        <li class="nav-item"><a class="nav-link" href='<%= ResolveUrl("~/Games/ColonyRuler/SettingsApi.aspx")%>'>Settings</a></li>
                        <li class="nav-item"><a class="nav-link" href='<%= ResolveUrl("~/Games/ColonyRuler/GetSaveNamesApi.aspx")%>'>Get save names list</a></li>
                        <li class="nav-item"><a class="nav-link" href='<%= ResolveUrl("~/Games/ColonyRuler/SaveApi.aspx")%>'>Save</a></li>
                    </ul>
                </div>
            </nav>
        </nav>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section class="section-margin">
    <div class="container">
      <div class="section-intro pb-85px text-center">
        <section class="core-rail">
            <section class="profile">
                <section class="top-card-layout top-card-layout--show-current-hide-past-position">
                    <figure class="cover-img onload lazy-loaded"></figure>
                    <div class="top-card-layout__card">
                        <br/>
                        <div class="top-card-layout__entity-info-container">
                            <div class="top-card-layout__entity-info">
                                <h1 class="top-card-layout__title">Get save names Api</h1>
                                This reguest require login. if a user is not loggined, the login could be "Not authorized"<br/>
                                It has only one type of reguest: Get<br/>
                                Get: <br/>
                                Parameter: login - your login name. Not secure yet. <br/> 
                                Return: JSON-string with serialised array of save names<br/>
                                Example: <br/>
                                <a href="https://ckpyt.com/api/getsavenames?login=Not authorized">https://ckpyt.com/api/getsavenames?login=Not authorized</a><br/>
                                returns: "[\"ssss\",\"ssss1\",\"1111\"]" <br/>
                            </div>
                        </div>
                    </div>
                </section>
            </section>
        </section>
      </div>
    </div>
 </section>
</asp:Content>