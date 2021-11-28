<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="future.aspx.cs" Inherits="WebAssessment.feature" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <!--================ Hero sm Banner start =================-->      

      <div class="hero-banner--sm__content">
        <h1>My future gamedev plans</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Future plans</li>
          </ol>
        </nav>
      </div>

  <!--================ Hero sm Banner end =================-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <!--================ Offer section start =================-->      
  <section class="section-margin">
    <div class="container">
      <div class="section-intro pb-85px text-center">
        <h2 class="section-intro__title">Colony Ruler development plan</h2>
        <p class="section-intro__subtitle">Colony Ruler is managing game, there a player could manage all the productions of the one colony. I'm trying to keep as abstract it as it possible. So, you don't need to manage a single person, only productions</p>
      </div>

      <div class="row">
        <div class="col-lg-6">

          <div class="row offer-single-wrapper">
              <div class="card offer-single__content text-center">
                <span class="offer-single__icon">
                  <i class="ti-rocket"></i>
                </span>
                <h4>First stage - prototype</h4>

                  <p>+Resources, manage workers and production tree</p>
                  <p>+Tools, broken tools and repair them</p>
                  <p>+Population and rising, if they happy and have enough houses</p>
                  <p>+Science and open researched items</p>
                  <p>+Speed: +/-, pause</p>
                  <p>+Warehouses and upgrades</p>
                  <p>+Happy, consuming  happy items(cloth, statues. etc), food</p>
                  <p>+Save/Load, settings, main menu</p>
                  <p>+Localization(now En/ru only)</p>
                  <p>+Full stone age</p>
                  <p>+Networking and site integration</p>
                  <p> <br/> Does not finished yet<br/></p>
                  <p>-Neolithic revolution(farming + taming animals)</p>
                  <p>-Tutorial</p>
                  <p>-History of the colony</p>
                  <p>-Steam, VKontakte integration</p>
              </div>
          </div>

          <div class="row offer-single-wrapper">
              <div class="card offer-single__content text-center">
                <span class="offer-single__icon">
                  <i class="ti-map-alt"></i>
                </span>
                <h4>Second stage - map and war</h4>
                <p>-Statistic screen with all learned materials, tools, buildings, etc</p>
                <p>-Map: 45*45, land size depends on latitude </p>
                <p>-Mine and upgrade them</p>
                <p>-Moving resources on the map. Loaders, carts, etc.</p>
                <p>-People can migrate to the happest place </p>
                <p>-Army: training, healing, attack</p>
                <p>-Military buildings: fort, castle, citadel</p>
                <p>-Copper age and bronze age.</p>
                <p>(the full list could be increased)</p>
              </div>
          </div>
          <div class="row offer-single-wrapper">
              <div class="card offer-single__content text-center">
                <span class="offer-single__icon">
                  <i class="ti-heart-broken"></i>
                </span>
                <h4>Third stage - population</h4>
                <p>-Population screen</p>
                  <p>-Population split by age and sex</p>
                  <p>-Deaths and born depends on the medecine level</p>
                  <p>-Diseases and medicines</p>
                  <p>-The number of newborns depends on the number of women <br/>aged 14  to 40 years.</p>
                  <p>-Schools, universities, church, temples, wonders of the world</p>
                  <p>-Speed of reasearch depends on the education level</p>
                  <p>-The number of newborns depends on the education level,  <br/>religion level, crowded population</p>
                  <p>-Iron age, Middle Ages</p>
                  <p>(the full list could be increased)</p>
              </div>
          </div>

        </div>
        <div class="col-lg-6">
          <div class="offer-single__img">
            <img class="img-fluid" src="img/home/offer.png" alt=""/>
          </div>
        </div>
      </div>
    </div>
  </section>
  <!--================ Offer section end =================-->
 

</asp:Content>
