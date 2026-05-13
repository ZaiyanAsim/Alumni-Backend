namespace Alumni_Portal.RAID.Services.Email
{
    internal class RegistrationApprovalEmailBuilder
    {
        internal static string BuildSubject(string firstName, string lastName)
            => $"Welcome to FAST ALCOM — Your account is ready, {firstName}!";

        internal static string BuildBody(string firstName, string lastName, string userType)
        {
            var loginUrl    = "http://localhost:4200/login";
            var accentColor = "#002147";
            var accentSoft  = "#e8edf3";
            var accentDeep  = "#001530";

            var roleLabel = userType.ToLower() switch
            {
                "alumni"     => "Alumni",
                "supervisor" => "Supervisor",
                _            => "Student",
            };

            var roleDescription = userType.ToLower() switch
            {
                "alumni"     => "You can now explore projects, connect with fellow alumni, and engage with the FAST community.",
                "supervisor" => "You can now manage and mentor projects, review student submissions, and engage with the FAST community.",
                _            => "You can now browse projects, collaborate with peers, and engage with the FAST community.",
            };

            return $$"""
    <!DOCTYPE html>
    <html lang="en">
    <head>
      <meta charset="UTF-8">
      <meta name="viewport" content="width=device-width, initial-scale=1.0">
    </head>
    <body style="margin:0;padding:0;background:#f4f2ee;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI','Helvetica Neue',Helvetica,Arial,sans-serif;color:rgba(0,0,0,0.9);-webkit-font-smoothing:antialiased;">
    <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f2ee;padding:32px 16px;">
    <tr><td align="center">
    <table width="620" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:8px;overflow:hidden;border:1px solid rgba(0,0,0,0.08);">

      <!-- Header -->
      <tr>
        <td style="padding:24px 32px 20px;border-bottom:1px solid rgba(0,0,0,0.08);">
          <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
              <td>
                <p style="margin:0 0 4px;font-size:12px;font-weight:600;color:{{accentColor}};">FAST ALCOM</p>
                <p style="margin:0;font-size:12px;color:rgba(0,0,0,0.6);">Account Approved</p>
              </td>
              <td align="right" valign="top">
                <span style="display:inline-block;background:#dcfce7;color:#166534;font-size:12px;font-weight:600;padding:4px 12px;border-radius:16px;">Approved</span>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Title -->
      <tr>
        <td style="padding:28px 32px 8px;">
          <h1 style="margin:0 0 8px;font-size:24px;font-weight:600;color:rgba(0,0,0,0.9);line-height:1.3;">Welcome to FAST ALCOM, {{firstName}}!</h1>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.6);line-height:1.5;">Your registration has been approved by an administrator.</p>
        </td>
      </tr>

      <!-- Role badge -->
      <tr>
        <td style="padding:16px 32px 0;">
          <table cellpadding="0" cellspacing="0" style="background:{{accentSoft}};border-radius:8px;width:100%;">
            <tr>
              <td style="padding:18px 22px;">
                <table cellpadding="0" cellspacing="0">
                  <tr>
                    <td style="padding-right:14px;vertical-align:middle;">
                      <span style="display:inline-block;background:{{accentColor}};color:#ffffff;font-size:12px;font-weight:600;padding:4px 14px;border-radius:16px;">{{roleLabel}}</span>
                    </td>
                    <td style="vertical-align:middle;">
                      <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.5;">{{roleDescription}}</p>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Greeting body -->
      <tr>
        <td style="padding:24px 32px 0;">
          <p style="margin:0 0 12px;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.6;">Your account has been created using the email address <span style="color:rgba(0,0,0,0.9);font-weight:600;">you registered with</span>. Use your registration password to log in.</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.6;">Click the button below to go to the portal login page:</p>
        </td>
      </tr>

      <!-- Login button -->
      <tr>
        <td style="padding:24px 32px 0;" align="center">
          <a href="{{loginUrl}}"
             style="display:inline-block;background:{{accentColor}};color:#ffffff;font-size:15px;font-weight:600;text-decoration:none;padding:13px 36px;border-radius:8px;letter-spacing:0.01em;">
            Log In to FAST ALCOM
          </a>
        </td>
      </tr>

      <!-- Sign-off -->
      <tr>
        <td style="padding:28px 32px 28px;">
          <p style="margin:0 0 16px;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.55;">If you have any trouble logging in, please contact the Alumni Portal support team.</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);">Regards,<br><span style="color:rgba(0,0,0,0.9);font-weight:600;">The FAST ALCOM Team</span></p>
        </td>
      </tr>

      <!-- Footer -->
      <tr>
        <td style="background:#f4f2ee;padding:16px 32px;border-top:1px solid rgba(0,0,0,0.08);">
          <p style="margin:0;font-size:12px;color:rgba(0,0,0,0.6);text-align:center;">This is an automated notification. Please do not reply to this email.</p>
        </td>
      </tr>

    </table>
    </td></tr>
    </table>
    </body>
    </html>
    """;
        }
    }
}
