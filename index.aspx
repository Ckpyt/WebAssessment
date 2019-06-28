<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebAssessment.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <!--================ Hero sm Banner start =================-->      
   
        <div class="row">
          <div class="col-lg-7">
            <div class="hero-banner__img">
              <img class="img-fluid" src="img/banner/hero-banner.png" alt="">
            </div>
          </div>
          <div class="col-lg-5 pt-5">
            <div class="hero-banner__content">
              <h1>Advertisemts from users</h1>
              <p>Here you can see some adverts by category or search</p>
              
            </div>
          </div>
        </div>

    <!--================ Hero sm Banner end =================-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="row">
          <div class="col-lg-7">
              <div class="hero-banner__content">
              <asp:DropDownList ID="AdsCategory" runat="server"></asp:DropDownList>
              <asp:TextBox ID="searchText" runat="server"></asp:TextBox>
              <asp:Button ID="btnSearch" runat="server" Text="search" OnClick="btnSearch_Click" />
              </div>
          </div>
       </div>

          <div class="col-lg-7">
              <div class="hero-banner__content">
                  <table>
                      <tr>
                          <td>
                              Name
                          </td>
                          <td>
                              Category
                          </td>
                          <td>
                              Text
                          </td>
                      </tr>
                    <tr>
                        <td colspan="3">
                           <hr />
                        </td>
                    </tr>
                  <asp:Repeater ID="rptAds" runat="server">
                       <ItemTemplate>
                           <tr>
                               <td>
                                   | <%# Eval("AdsName") %>
                               </td>
                               <td>
                                   | <%# Eval("Name") %>  
                               </td>
                               <td>
                                   | <%# Eval("AdsText") %>
                               </td>
                           </tr>
                       </ItemTemplate>
                      <SeparatorTemplate>
                          <tr>
                              <td colspan="3">
                                  _____________________________________________________________
                              </td>
                          </tr>
                      </SeparatorTemplate>
                  </asp:Repeater>
                  </table>
              </div>
          </div>

</asp:Content>
