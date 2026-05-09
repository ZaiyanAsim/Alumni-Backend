using Alumni_Portal.Engagement.Services.DTO;

namespace Alumni_Portal.RAID.Services.Email
{
    internal class RequestRejectionEmailBuilder
    {
        internal static string BuildSubject(RequestRejectionDTO dto)
        {
            return $"Request Update: [{dto.Request_Type_Value}] [{dto.Project_Academic_ID}] [{dto.Project_Name}]";
        }

        internal static string BuildBody(RequestRejectionDTO dto)
        {
            var firstName = dto.Individual_Name?.Split(' ')[0] ?? dto.Individual_Name;

            var accentColor = "#6b6b6b";
            var accentSoft = "#f3f2ef";
            var accentDeep = "#3d3d3d";
            var dividerLine = "rgba(0,0,0,0.08)";

            var reasonBlock = string.IsNullOrWhiteSpace(dto.Rejection_Reason)
                ? ""
                : $$"""
                <!-- Reason for rejection -->
                <tr>
                  <td style="padding:20px 32px 0;">
                    <p style="margin:0 0 10px;font-size:14px;font-weight:600;color:rgba(0,0,0,0.9);">Reason for rejection</p>
                    <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid {{dividerLine}};border-left:3px solid {{accentColor}};border-radius:8px;">
                      <tr>
                        <td style="padding:16px 18px;">
                          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.8);line-height:1.6;font-style:italic;">{{dto.Rejection_Reason}}</p>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                """;

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
                <p style="margin:0 0 4px;font-size:12px;font-weight:600;color:{{accentColor}};">Alumni Portal</p>
                <p style="margin:0;font-size:12px;color:rgba(0,0,0,0.6);">Request Decision</p>
              </td>
              <td align="right" valign="top">
                <span style="display:inline-block;background:{{accentSoft}};color:{{accentDeep}};font-size:12px;font-weight:600;padding:4px 12px;border-radius:16px;border:1px solid {{dividerLine}};">Not Approved</span>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Title -->
      <tr>
        <td style="padding:28px 32px 4px;">
          <h1 style="margin:0 0 8px;font-size:24px;font-weight:600;color:rgba(0,0,0,0.9);line-height:1.3;">An update on your request</h1>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.6);line-height:1.5;">{{dto.Project_Name}}</p>
        </td>
      </tr>

      <!-- Meta bar -->
      <tr>
        <td style="padding:20px 32px 0;">
          <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f2ee;border-radius:8px;">
            <tr>
              <td style="padding:14px 18px;">
                <p style="margin:0 0 3px;font-size:11px;color:rgba(0,0,0,0.6);font-weight:400;">Request ID</p>
                <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Request_ID}}</p>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Greeting -->
      <tr>
        <td style="padding:24px 32px 0;">
          <p style="margin:0 0 12px;font-size:16px;font-weight:600;color:rgba(0,0,0,0.9);">Hi {{firstName}},</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.55;">Thank you for submitting your <span style="color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Request_Type_Value}}</span> request for <span style="color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Project_Name}}</span>. After careful review, we regret to inform you that your request has <span style="color:rgba(0,0,0,0.9);font-weight:600;">not been approved</span> at this time.</p>
        </td>
      </tr>

      <!-- Summary -->
      <tr>
        <td style="padding:20px 32px 0;">
          <p style="margin:0 0 10px;font-size:14px;font-weight:600;color:rgba(0,0,0,0.9);">Request details</p>
          <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid {{dividerLine}};border-radius:8px;">
            <tr>
              <td style="padding:4px 18px;">
                <table width="100%" cellpadding="0" cellspacing="0" style="font-size:14px;">
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);width:160px;border-bottom:1px solid {{dividerLine}};">Full name</td>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.9);font-weight:600;border-bottom:1px solid {{dividerLine}};text-align:right;">{{dto.Individual_Name}}</td>
                  </tr>
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);border-bottom:1px solid {{dividerLine}};">Project name</td>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.9);font-weight:600;border-bottom:1px solid {{dividerLine}};text-align:right;">{{dto.Project_Name}}</td>
                  </tr>
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);border-bottom:1px solid {{dividerLine}};">Project ID</td>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.9);font-weight:600;border-bottom:1px solid {{dividerLine}};text-align:right;">{{dto.Project_Academic_ID}}</td>
                  </tr>
                  <tr>
                    <td style="padding:12px 0;color:rgba(0,0,0,0.6);">Request type</td>
                    <td style="padding:12px 0;text-align:right;">
                      <span style="background:{{accentSoft}};color:{{accentDeep}};font-size:12px;padding:3px 10px;border-radius:12px;font-weight:600;border:1px solid {{dividerLine}};">{{dto.Request_Type_Value}}</span>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      {{reasonBlock}}

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
                    <td style="padding:5px 0;">Review the feedback provided and consider whether the concerns can be addressed.</td>
                  </tr>
                  <tr>
                    <td valign="top" style="padding:5px 12px 5px 0;color:{{accentColor}};font-weight:600;">2.</td>
                    <td style="padding:5px 0;">If eligible, you may submit a <span style="color:rgba(0,0,0,0.9);font-weight:600;">revised request</span> through the Alumni Portal.</td>
                  </tr>
                  <tr>
                    <td valign="top" style="padding:5px 12px 5px 0;color:{{accentColor}};font-weight:600;">3.</td>
                    <td style="padding:5px 0;">For clarification on this decision, please contact the Alumni Portal support team.</td>
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
          <p style="margin:0 0 16px;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.55;">We appreciate the time and effort you put into your submission, and we encourage you to stay engaged with the Alumni Portal community.</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);">Regards,<br><span style="color:rgba(0,0,0,0.9);font-weight:600;">The Alumni Portal Team</span></p>
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