<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="not-authenticated.aspx.cs" Inherits="web.not_authenticated" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Ej behörig! <br />
            <br />
            Alternativt och kontroll kan vara att autentiseringsmetoder för webbapplikationen i IIS:n är "Windows-autentisering". "Anonym autentisering" ska vara inaktiverad.
        </div>
    </form>
</body>
</html>
