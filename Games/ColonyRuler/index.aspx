<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebAssessment.Games.ColonyRuler.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <link rel="stylesheet" href="TemplateData/style.css">
    <script src="TemplateData/UnityProgress.js"></script>
    <script src="Build/UnityLoader.js"></script>
    <script>
        var unityInstance = UnityLoader.instantiate("unityContainer", "Build/WebGL.json", { onProgress: UnityProgress });
        function GetUserName() {
            return '<%= UserName %>';
        }
        unityInstance.SendMessage('MainMenu', 'StoreUserName', '<%= UserName %>');
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <div class="hero-banner--sm__content">
        <h1>Colony ruler</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Colony ruler</li>
          </ol>
        </nav>
      </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div style="width: 960px; height: 670px; position:relative; left: 50%;
     -moz-transform: translateX(-50%);
    -webkit-transform: translateX(-50%);
    transform: translateX(-50%)">
       <div class="webgl-content">
          <div id="unityContainer" style="width: 960px; height: 600px"></div>
          <div class="footer">
            <div class="webgl-logo"></div>
            <div class="fullscreen" onclick="unityInstance.SetFullscreen(1)"></div>
            <div class="title">ColonyRuler</div>
          </div>
        </div>
    </div>
</asp:Content>
