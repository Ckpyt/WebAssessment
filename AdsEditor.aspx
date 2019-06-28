<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdsEditor.aspx.cs" Inherits="WebAssessment.AdsEditor" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!--================ Hero sm Banner start =================-->      
    

      <div class="hero-banner--sm__content">
        <h1>Editor of your adverts</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Ads Editor</li>
          </ol>
        </nav>
      </div>

  <!--================ Hero sm Banner end =================-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container text-center centered-element">
        <table class="container text-center centered-element">
            <tr>
                <td>
                    Your ads:
                                
                     <div class="container centered-element">
                      <table>
                        <asp:Repeater ID="rptAds" runat="server">
                            <ItemTemplate>

                                <tr>
                                    <td>
                                        Name: <%# Eval("AdsName")%>
                                    </td>
                                    <td>
                                        <asp:Button  class="button blue-btn  ibvm" CausesValidation="False" CommandArgument='<%#((string)Eval("Id").ToString())%>' ID="btnAdsEdit" runat="server"  Text="Edit"  EnableViewState="True" OnClick="EditAds_Click" />
                                    </td>
                                    <td>
                                        <asp:Button  class="button blue-btn  ibvm" CausesValidation="False" CommandArgument='<%#((string)Eval("Id").ToString())%>' ID="btnAdsDelete" runat="server"  Text="Delete"  EnableViewState="True" OnClick="DeleteAds_Click" />
                                    </td>
                                </tr>
                             
                            </ItemTemplate>
                        </asp:Repeater>
                        </table>
                    </div>
            

                </td>
                <td>
                    <table class="container text-center centered-element">
                        <tr>
                            <td>
                                Ads name:
                            </td>
                            <td>
                                <asp:TextBox ID="AdsName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ads category:
                            </td>
                            <td>
                                <asp:DropDownList ID="AdsCategory" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ads text:
                            </td>
                            <td>
                                <asp:TextBox ID="AdsText" runat="server" TextMode="MultiLine" Height="180px" Width="255px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="AdsEnabled" runat="server" Text="Ads enabled" Visible="false" />
                                <asp:Label runat="server" ID="AdvId" CssClass="invisible"> </asp:Label>
                                <asp:Button class="button blue-btn  ibvm" ID="EditAds" runat="server" Text="Add new ads" OnClick="EditAdsBtn_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
