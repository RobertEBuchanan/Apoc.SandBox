<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreatePFD.aspx.cs" Inherits="Apoc.PdfCreator.CreatePFD" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Create PDF from HTML</h1>
    <hr />
    <div style="width: 800px;">
    <asp:TextBox ID="txtInputHTML" runat="server" TextMode="MultiLine" Width="600" Height="300" style="max-width: 1000px;"></asp:TextBox>
        <br />
    <asp:Button ID="cmdMakePDF" runat="server" Text="Make PDF" OnClick="cmdMakePDF_Click" />
    </div>
</asp:Content>
