<%@ Page Language="C#" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";

        Response.Write("=== ASP.NET Request Info ===\n");
        Response.Write("RawUrl (original path before rewrite): " + Request.RawUrl + "\n");
        Response.Write("Url (current request to ASP.NET): " + Request.Url + "\n\n");

        Response.Write("=== Server Variables ===\n");
        foreach (string key in Request.ServerVariables.AllKeys)
        {
            Response.Write(key + " = " + Request.ServerVariables[key] + "\n");
        }
    }
</script>
