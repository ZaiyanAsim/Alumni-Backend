namespace Alumni_Portal.RAID.Services.Email
{
    internal class RegistrationRejectionEmailBuilder
    {
        internal static string BuildSubject(string firstName, string lastName)
            => $"Your FAST ALCOM registration request has been reviewed";

        internal static string BuildBody(string firstName, string lastName)
        {
            var accentColor  = "#6b6b6b";
            var accentSoft   = "#f3f2ef";
            var accentDeep   = "#3d3d3d";
            var dividerLine  = "rgba(0,0,0,0.08)";

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
        <td style="padding:24px 32px 20px;border-bottom:1px solid {{dividerLine}};">
          <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
              <td>
                <p style="margin:0 0 4px;font-size:12px;font-weight:600;color:{{accentColor}};">FAST ALCOM</p>
                <p style="margin:0;font-size:12px;color:rgba(0,0,0,0.6);">Registration Decision</p>
              </td>
              <td align="right" valign="top">
                <span style="display:inline-block;background:#fee2e2;color:#991b1b;font-size:12px;font-weight:600;padding:4px 12px;border-radius:16px;">Not Approved</span>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Title -->
      <tr>
        <td style="padding:28px 32px 4px;">
          <h1 style="margin:0 0 8px;font-size:24px;font-weight:600;color:rgba(0,0,0,0.9);line-height:1.3;">An update on your registration</h1>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.6);line-height:1.5;">FAST ALCOM Alumni Portal</p>
        </td>
      </tr>

      <!-- Greeting -->
      <tr>
        <td style="padding:24px 32px 0;">
          <p style="margin:0 0 12px;font-size:16px;font-weight:600;color:rgba(0,0,0,0.9);">Hi {{firstName}},</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.6;">Thank you for taking the time to register with FAST ALCOM. After reviewing your request, we regret to inform you that your registration has <span style="color:rgba(0,0,0,0.9);font-weight:600;">not been approved</span> at this time.</p>
        </td>
      </tr>

      <!-- What you can do next -->
      <tr>
        <td style="padding:20px 32px 0;">
          <table width="100%" cellpadding="0" cellspacing="0" style="background:{{accentSoft}};border-radius:8px;">
            <tr>
              <td style="padding:18px 22px;">
                <p style="margin:0 0 12px;font-size:14px;font-weight:600;color:{{accentDeep}};">What you can do next</p>
                <table cellpadding="0" cellspacing="0" style="font-size:14px;color:rgba(0,0,0,0.8);line-height:1.55;">
                  <tr>
                    <td valign="top" style="padding:5px 12px 5px 0;color:{{accentColor}};font-weight:600;">1.</td>
                    <td style="padding:5px 0;">Ensure the details you provided (roll number, email, department) match your official academic records.</td>
                  </tr>
                  <tr>
                    <td valign="top" style="padding:5px 12px 5px 0;color:{{accentColor}};font-weight:600;">2.</td>
                    <td style="padding:5px 0;">You may submit a new registration request through the portal if you believe there was an error.</td>
                  </tr>
                  <tr>
                    <td valign="top" style="padding:5px 12px 5px 0;color:{{accentColor}};font-weight:600;">3.</td>
                    <td style="padding:5px 0;">For further clarification, please contact the FAST ALCOM support team directly.</td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Sign-off -->
      <tr>
        <td style="padding:24px 32px 28px;">
          <p style="margin:0 0 16px;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.55;">We appreciate your interest in the FAST ALCOM community and hope to see you again.</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);">Regards,<br><span style="color:rgba(0,0,0,0.9);font-weight:600;">The FAST ALCOM Team</span></p>
        </td>
      </tr>

      <!-- Footer -->
      <tr>
        <td style="background:#f4f2ee;padding:16px 32px;border-top:1px solid {{dividerLine}};">
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
