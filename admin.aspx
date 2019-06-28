<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="WebAssessment.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <!--================ Hero sm Banner start =================-->      
    
    <script type="text/javascript">
        function DisplayModal(id) {
            document.getElementById("overlay-" + id).style.height = document.body.clientHeight + 'px';
            document.getElementById("overlay-" + id).className = "OverlayEffect";
            document.getElementById("modalMsg-" + id).className = "ShowModal";
            return false;
        };

        function RemoveModal(id) {
            document.getElementById("overlay-" + id).style.height = "0px";
            document.getElementById("overlay-" + id).className = "";
            document.getElementById("modalMsg-" + id).className = "HideModal";
        }
      </script>
      <div class="hero-banner--sm__content">
        <h1>Admin page</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Admin page</li>
          </ol>
        </nav>
      </div>

  <!--================ Hero sm Banner end =================-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table>
        <tr>
            <td>
        <div class="container centered-element">
            <table>
                <tr>
                    <td>
                        <div class="nav-link tbl"> Login: </div>
                    </td>
                    <td>
                        <asp:Label id="Login" runat="server" >Noone selected</asp:Label>
                    </td>
                </tr>
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
                        <div class="nav-link tbl"> User's description: </div>
                    </td>
                    <td>
                        <asp:TextBox id="Description" runat="server" Height="132px" TextMode="MultiLine" Width="229px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="DisableAccount" runat="server" Text="Disable Account" CausesValidation="True" />
                    </td>
                    <td>
                        Until:
                        <asp:TextBox id="Time" runat="server" TextMode="Date" Width="200px" Heigth="50px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="UpdateAccount" class="button blue-btn  ibvm" runat="server" Text="Update Account" OnClick="UpdateAccount_Click" />
                    </td>
                </tr>
            </table>

        </div>
            </td>
            <td>
        <div class="container centered-element">
        <asp:Repeater ID="rptUsers" runat="server">
    
                    <ItemTemplate>
                        <div class="container centered-element">
                        <table class="centered-element">
                            <tr>
                                <td>Name
                                </td>
                                <td>
                                    <%# Eval("UserName") %>
                                </td>
                            </tr>
                            <tr>
                                <td>Email
                                </td>
                                <td>
                                    <asp:Label ID="Email" runat="server"><%# Eval("Email") %></asp:Label>
                                
                                </td>
                            </tr>
                            <tr>
                                <td>Description:
                                </td>
                            
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDescription" runat="server" ><%# Eval("Description") %></asp:Label>
                               
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="2">
                                   <asp:Button  class="button blue-btn  ibvm" ID="btnDeleteClient" runat="server" Text="Delete"  OnClientClick='<%# "return DisplayModal(\"" +  ((string) Eval("Id")) + "\")"%>' />
                                   <asp:Button  class="button blue-btn  ibvm" ID="btnShowUser" runat="server" Text="Show User" CommandArgument='<%#Eval("Id")%>' OnClick="ShowUser" />
                                </td>

                            </tr>
                        </table>
                        </div>
                        <%--Start Delete Modal Popup--%>

                        <div id='overlay-<%# Eval("Id")%>'></div>
                        <div id='modalMsg-<%# Eval("Id")%>' class="HideModal">

                            <div class="popupbox">
                                <br />
                                <div class="popupttl bluetxt"><b>Delete Confirmation</b></div>
                                <hr />
                                <br />
                                <br />
                                <div class="popupdesc">
                                    <!-- START POPUP BLOCK -->
                                    <div class="popblock">
                                        <div class="poptxt">
                                            Are you sure you want to delete?
                                        </div>
                                    </div>
                                    <!-- END POPUP BLOCK -->
                                    <!-- START POPUP BLOCK -->
                                    <br />

                                    <div class="popblock popblocksubbtn">
                                        <asp:Button  class="button blue-btn  ibvm" CausesValidation="False" CommandArgument='<%#((string)Eval("Id"))%>' ID="btnDelete" runat="server"  Text="Yes" OnClick="ConfirmButton_Click" />
                                        <input type="submit" value="No" class="button bluebg ibvm" onclick='<%# "RemoveModal(\"" +  ((string) Eval("Id")) + "\"))" %>' />
                                        <asp:Button  class="button blue-btn  ibvm" ID="btnHide" runat="server" Text="Back" Style="display: none" />
                                    </div>
                                    <!-- END POPUP BLOCK -->
                                </div>
                            </div>
                        </div>

                        <%--End of Delete Modal Poup--%>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <hr />
                    </SeparatorTemplate>
            
        </asp:Repeater>
        </div>
            </td>
            <td>
                <div class="container centered-element">
                                         <table>
                <asp:Repeater ID="rptCategory" runat="server">
                    <ItemTemplate>

                        <tr>
                            <td>
                                | ID: <%# Eval("Id")%>
                            </td>
                            <td>
                                | Name: <%# Eval("Name")%>
                            </td>
                            <td>
                                |<asp:Button  class="button blue-btn  ibvm" CausesValidation="False" CommandArgument='<%#((string)Eval("Id").ToString())%>' ID="btnEdit" runat="server"  Text="Edit categiry" OnClick="EditCategoryButton_Click" />
                            
                            </td>
                        </tr>

                    </ItemTemplate>
                    <SeparatorTemplate>
                        <tr>
                            <td colspan ="3">
                               <hr />
                            </td>
                        </tr>
                    </SeparatorTemplate>
                </asp:Repeater>
                       </table>
                    <asp:Label ID="CatId" runat="server" class="invisible"></asp:Label>
                    Category name: <asp:TextBox ID="Category" runat="server"></asp:TextBox>
                    <asp:Button ID="EditCat" class="button blue-btn  ibvm" runat="server" Text="Add" OnClick="EditCat_Click" />
            </div>
            </td>
        </tr>

        </table>
    </div>
</asp:Content>
