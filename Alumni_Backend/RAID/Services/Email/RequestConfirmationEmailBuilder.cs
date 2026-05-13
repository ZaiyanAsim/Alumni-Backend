using Alumni_Portal.Engagement.Services.DTO;

namespace Alumni_Portal.RAID.Services.Email
{
    internal class RequestConfirmationEmailBuilder
    {
        internal static string BuildSubject(RequestDTO dto)
        {
            return $"Request Received: [{dto.Request_Type_Value}] [{dto.Project_Academic_ID}] [{dto.Project_Name}]";

        }

        internal static string BuildBody(RequestDTO dto, int requestId)
        {
            var firstName = dto.Individual_Name?.Split(' ')[0] ?? dto.Individual_Name;

            var accentColor = "#057642";
            var accentSoft = "#ddf4e4";
            var accentDeep = "#044d2c";
            var formattedDate = dto.Created_At.ToString("dd MMM yyyy, HH:mm 'UTC'");

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
                <p style="margin:0 0 4px;font-size:12px;font-weight:600;color:{{accentColor}};">Alumni Portal</p>
                <p style="margin:0;font-size:12px;color:rgba(0,0,0,0.6);">Request Confirmation</p>
              </td>
              <td align="right" valign="top">
                <span style="display:inline-block;background:{{accentSoft}};color:{{accentDeep}};font-size:12px;font-weight:600;padding:4px 12px;border-radius:16px;">{{dto.Request_Type_Value}}</span>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Title -->
      <tr>
        <td style="padding:28px 32px 4px;">
          <h1 style="margin:0 0 8px;font-size:24px;font-weight:600;color:rgba(0,0,0,0.9);line-height:1.3;">We've received your request</h1>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.6);line-height:1.5;">{{dto.Project_Name}}</p>
        </td>
      </tr>

      <!-- Meta bar -->
      <tr>
        <td style="padding:20px 32px 0;">
          <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f2ee;border-radius:8px;">
            <tr>
              <td style="padding:14px 18px;width:50%;">
                <p style="margin:0 0 3px;font-size:11px;color:rgba(0,0,0,0.6);font-weight:400;">Request ID</p>
                <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.9);font-weight:600;">{{requestId}}</p>
              </td>
              <td style="padding:14px 18px;width:50%;">
                <p style="margin:0 0 3px;font-size:11px;color:rgba(0,0,0,0.6);font-weight:400;">Submitted</p>
                <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Created_At.Date}}</p>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Greeting -->
      <tr>
        <td style="padding:24px 32px 0;">
          <p style="margin:0 0 12px;font-size:16px;font-weight:600;color:rgba(0,0,0,0.9);">Hi {{firstName}},</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.55;">Thank you for submitting your <span style="color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Request_Type_Value}}</span> request for <span style="color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Project_Name}}</span>. Your submission has been received and forwarded to the relevant administrators for review.</p>
        </td>
      </tr>

      <!-- Summary -->
      <tr>
        <td style="padding:20px 32px 0;">
          <p style="margin:0 0 10px;font-size:14px;font-weight:600;color:rgba(0,0,0,0.9);">Submission summary</p>
          <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid rgba(0,0,0,0.08);border-radius:8px;">
            <tr>
              <td style="padding:4px 18px;">
                <table width="100%" cellpadding="0" cellspacing="0" style="font-size:14px;">
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);width:160px;border-bottom:1px solid rgba(0,0,0,0.08);">Full name</td>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.9);font-weight:600;border-bottom:1px solid rgba(0,0,0,0.08);text-align:right;">{{dto.Individual_Name}}</td>
                  </tr>
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);border-bottom:1px solid rgba(0,0,0,0.08);">Project name</td>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.9);font-weight:600;border-bottom:1px solid rgba(0,0,0,0.08);text-align:right;">{{dto.Project_Name}}</td>
                  </tr>
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);border-bottom:1px solid rgba(0,0,0,0.08);">Project ID</td>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.9);font-weight:600;border-bottom:1px solid rgba(0,0,0,0.08);text-align:right;">{{dto.Project_Academic_ID}}</td>
                  </tr>
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);border-bottom:1px solid rgba(0,0,0,0.08);">Request type</td>
                    <td style="padding:12px 0;text-align:right;border-bottom:1px solid rgba(0,0,0,0.08);">
                      <span style="background:{{accentSoft}};color:{{accentDeep}};font-size:12px;padding:3px 10px;border-radius:12px;font-weight:600;">{{dto.Request_Type_Value}}</span>
                    </td>
                  </tr>
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);">Submitted at</td>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.9);font-weight:600;text-align:right;">{{formattedDate}}</td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- What happens next -->
      <tr>
        <td style="padding:20px 32px 0;">
          <table width="100%" cellpadding="0" cellspacing="0" style="background:{{accentSoft}};border-radius:8px;">
            <tr>
              <td style="padding:18px 22px;">
                <p style="margin:0 0 12px;font-size:14px;font-weight:600;color:{{accentDeep}};">What happens next</p>
                <table cellpadding="0" cellspacing="0" style="font-size:14px;color:rgba(0,0,0,0.8);line-height:1.55;">
                  <tr>
                    <td valign="top" style="padding:5px 12px 5px 0;color:{{accentColor}};font-weight:600;">1.</td>
                    <td style="padding:5px 0;">Administrators will review your request within <span style="color:rgba(0,0,0,0.9);font-weight:600;">3&ndash;5 business days.</span></td>
                  </tr>
                  <tr>
                    <td valign="top" style="padding:5px 12px 5px 0;color:{{accentColor}};font-weight:600;">2.</td>
                    <td style="padding:5px 0;">You will receive an email notification once a decision has been made.</td>
                  </tr>
                  <tr>
                    <td valign="top" style="padding:5px 12px 5px 0;color:{{accentColor}};font-weight:600;">3.</td>
                    <td style="padding:5px 0;">If further information is required, an administrator will reach out directly.</td>
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
          <p style="margin:0 0 16px;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.55;">For any questions, please contact the Alumni Portal support team.</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);">Regards,<br><span style="color:rgba(0,0,0,0.9);font-weight:600;">The Alumni Portal Team</span></p>
        </td>
      </tr>

      <!-- Footer -->
      <tr>
        <td style="background:#f4f2ee;padding:16px 32px;border-top:1px solid rgba(0,0,0,0.08);">
          <p style="margin:0;font-size:12px;color:rgba(0,0,0,0.6);text-align:center;">This is an automated confirmation. Please do not reply to this email.</p>
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
