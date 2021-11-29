<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GetLocalizationApi.aspx.cs" Inherits="WebAssessment.Games.GetLocalizationApi" %>
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
                                <h1 class="top-card-layout__title">Get localisation Api</h1>
                                This reguest does not require login and can be done by anyone. <br/>
                                There is two parametres: <br/>string "language", should be the same as it was recieve from GetLanguagesList request, could be skipped(English by default) <br/>
                                and id - could be 1-3. <br/>
                                id = 1: returns UI localisation string <br/>
                                id = 2: returns items localisation array <br/>
                                id = 3: return history localisation string(not realised yet) <br/>
                                Any others id returns UI localisation string <br/>
                                It has only one type of reguests: get. <br/>
                                Could be skipped, 1 by default<br/>
                                Example: <br/>
                                <a href="https://ckpyt.com/api/GetLocalisation?id=1&language=Russian">https://ckpyt.com/api/GetLocalisation?id=1&language=Russian</a><br/>
                                Return: <br/>
                                ""{\n\t\"m_newGame\": \"Новая игра\",\n\t\"m_loadGame\": \"Загрузить игру\",\n\t\"m_saveGame\": \"Сохранить игру\",\n\t\"m_settings\": \"Настройки\",\n\t\"m_about\": \"Об игре\",\n\t\"m_exit\": \"Выход\",\n\t\"m_resume\": \"Вернуться в игру\",\n\t\"m_exitToMainMenu\": \"Выйти в главное меню\",\n\t\"m_saveButton\": \"Сохранить\",\n\t\"m_loadButton\": \"Загрузить\",\n\t\"m_deleteButton\": \"Удалить\",\n\t\"m_rewriteButton\": \"Переписать\",\n\t\"m_applyButton\": \"OK\",\n\t\"m_cancelButton\": \"Отмена\",\n\t\"m_restoreButton\": \"По умолчанию\",\n\t\"m_fullTreeText\": \"Показывать полное дерево\",\n\t\"m_productitivityLenghtText\": \"Размер очереди производства  (больше - показывает без пиков)\",\n\t\"m_changeLanguageText\": \"Выбрать язык:\",\n    \"m_abstractCount\": \"Количество предметов на складе\",\n\t\"m_abstractProductivity\": \"Производительность: произведено - потреблено\",\n\t\"m_abstractDamaged\": \"не используется\",\n\t\"m_itemsDamaged\": \"Сломанные предметы\",\n\t\"m_scienceCount\": \"Прогресс исследования\",\n\t\"m_scienceProductivity\": \"Производительность: процент от полного потребления\",\n\t\"m_resourceCount\": \"Сколько здесь ресурса\",\n\t\"m_processCount\": \"Сколько полей созрело \\\\ растет\",\n\t\"m_emojiBoost\": \"К вам пришло СЧАСТЬЕ!\",\n\t\"m_emojiHappy\": \"Счастье:\",\n\t\"m_emojiMaxHappy\": \"макс. счастье:\",\n\t\"m_emojiMaxPopulation\": \"макс. население:\",\n\t\"m_starving\": \"Глад посетил вас! Накормите ваших людей, или они умрут!\",\n\t\"m_timePause\": \"Пауза\",\n\t\"m_timePlus\": \"Ускорить игру\",\n\t\"m_timeMinus\": \"Замедлить игру\",\n\t\"m_workersTooltip\": \"Используйте их чтобы что-то произвести\",\n\t\"m_populationTooltip\": \"Ваше население. Сделайте его счастливым, и новые люди к вам потянутся\",\n\t\"m_lightStorageTooltip\": \"Используется для улучшения производства маленьких вещей\",\n\t\"m_heavyStorageTooltip\": \"Используется для улучшения производства больших вещей\",\n\t\"m_livingSpaceTooltip\": \"Сколько людей вы можете иметь\",\n\t\"m_territoryTooltip\": \"Используется для строительства домов, складов и т.д.\",\n\t\"m_territoryText\": \"Территория\",\n\t\"m_heavyText\": \"Склад тяжелых предметов\",\n\t\"m_lightText\": \"Склад легких предметов\",\n\t\"m_livingText\": \"Жилое место\",\n\t\"m_timeText\": \"Время:\",\n\t\"m_dayText\": \"день:\",\n\t\"m_yearText\": \"год:\",\n\t\"m_pausedText\": \"пауза\",\n\t\"m_speedText\": \"скорость\",\n\t\"m_freeWorkersText\": \"не работают:\",\n\t\"m_populationText\": \"Население\"\n}"" <br/>
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
