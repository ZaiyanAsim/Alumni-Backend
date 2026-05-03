using Alumni_Portal.Engagement.Services.DTO;

namespace Alumni_Portal.RAID.Services.Email
{
    internal static class RequestNotificationEmailBuilder
    {
        public static string BuildSubject(RequestDTO dto)
        {

            return $"[{dto.Request_Type_Value}] [{dto.Project_Academic_ID}] {dto.Project_Name} · {dto.Individual_Academic_ID}";

        }

        internal static string BuildBody(RequestDTO dto, int requestId)
        {
            var accentColor = "#057642";
            var accentSoft = "#ddf4e4";
            var accentDeep = "#044d2c";

            var formattedDate = dto.Created_At.ToString("dd MMM yyyy, HH:mm 'UTC'");

        
            string Row(string label, string? value, bool isLast = false) =>
                string.IsNullOrWhiteSpace(value) ? "" : $$"""
            <tr>
              <td style="padding:12px 0;color:rgba(0,0,0,0.6);width:160px;{{(isLast ? "" : "border-bottom:1px solid rgba(0,0,0,0.08);")}}font-size:14px;">{{label}}</td>
              <td style="padding:12px 0;color:rgba(0,0,0,0.9);font-weight:600;{{(isLast ? "" : "border-bottom:1px solid rgba(0,0,0,0.08);")}}text-align:right;font-size:14px;">{{value}}</td>
            </tr>
            """;

            string LinkRow(string label, string? value, bool isLast = false) =>
                string.IsNullOrWhiteSpace(value) ? "" : $$"""
            <tr>
              <td style="padding:12px 0;color:rgba(0,0,0,0.6);width:160px;{{(isLast ? "" : "border-bottom:1px solid rgba(0,0,0,0.08);")}}font-size:14px;">{{label}}</td>
              <td style="padding:12px 0;{{(isLast ? "" : "border-bottom:1px solid rgba(0,0,0,0.08);")}}text-align:right;">
                <a href="{{value}}" style="color:{{accentColor}};text-decoration:none;font-weight:600;font-size:14px;word-break:break-all;">{{value}}</a>
              </td>
            </tr>
            """;

            var applicantRows =
                Row("Full name", dto.Individual_Name) +
                Row("Email", dto.Individual_Email) +
                Row("Contact", dto.Individual_Contact_Number) +
                LinkRow("LinkedIn", dto.Individual_LinkedIn_Url) +
                Row("Academic ID", dto.Individual_Academic_ID) +
                Row("Status", dto.Is_Individual_Registered ? "Registered alumnus" : "External applicant", isLast: true);

            var hasOrg = dto.Is_Organization && !string.IsNullOrWhiteSpace(dto.Organization_Name);
            var organizationBlock = !hasOrg ? "" : $$"""
        <tr>
          <td style="padding:20px 32px 0;">
            <p style="margin:0 0 10px;font-size:14px;font-weight:600;color:rgba(0,0,0,0.9);">Applying through organization</p>
            <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid rgba(0,0,0,0.08);border-radius:8px;">
              <tr>
                <td style="padding:4px 18px;">
                  <table width="100%" cellpadding="0" cellspacing="0">
                    {{Row("Organization", dto.Organization_Name, isLast: string.IsNullOrWhiteSpace(dto.Organization_Role))}}
                    {{Row("Role", dto.Organization_Role, isLast: true)}}
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
        """;

            var motivationBlock = string.IsNullOrWhiteSpace(dto.Motivation_Statement) ? "" : $$"""
        <tr>
          <td style="padding:20px 32px 0;">
            <p style="margin:0 0 10px;font-size:14px;font-weight:600;color:rgba(0,0,0,0.9);">Motivation statement</p>
            <table width="100%" cellpadding="0" cellspacing="0" style="background:{{accentSoft}};border-radius:8px;">
              <tr>
                <td style="padding:18px 22px;">
                  <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.8);line-height:1.7;font-style:italic;border-left:3px solid {{accentColor}};padding-left:14px;">"{{dto.Motivation_Statement}}"</p>
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
        <td style="padding:24px 32px 20px;border-bottom:1px solid rgba(0,0,0,0.08);">
          <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
              <td>
                <p style="margin:0 0 4px;font-size:12px;font-weight:600;color:{{accentColor}};">Alumni Portal</p>
                <p style="margin:0;font-size:12px;color:rgba(0,0,0,0.6);">Admin Notification &middot; Action Required</p>
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
          <h1 style="margin:0 0 8px;font-size:24px;font-weight:600;color:rgba(0,0,0,0.9);line-height:1.3;">New {{dto.Request_Type_Value}} request</h1>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.6);line-height:1.5;">{{dto.Project_Name}}</p>
        </td>
      </tr>

      <!-- Meta bar -->
      <tr>
        <td style="padding:20px 32px 0;">
          <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f2ee;border-radius:8px;">
            <tr>
              <td style="padding:14px 18px;width:50%;">
                <p style="margin:0 0 3px;font-size:11px;color:rgba(0,0,0,0.6);">Project ID</p>
                <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Project_Academic_ID}}</p>
              </td>
              <td style="padding:14px 18px;width:50%;">
                <p style="margin:0 0 3px;font-size:11px;color:rgba(0,0,0,0.6);">Submitted</p>
                <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.9);font-weight:600;">{{formattedDate}}</p>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Intro -->
      <tr>
        <td style="padding:24px 32px 0;">
          <p style="margin:0 0 12px;font-size:16px;font-weight:600;color:rgba(0,0,0,0.9);">Hello Admin,</p>
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);line-height:1.55;">A new <span style="color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Request_Type_Value}}</span> request has been submitted for <span style="color:rgba(0,0,0,0.9);font-weight:600;">{{dto.Project_Name}}</span> and requires your review.</p>
        </td>
      </tr>

      <!-- Applicant -->
      <tr>
        <td style="padding:20px 32px 0;">
          <p style="margin:0 0 10px;font-size:14px;font-weight:600;color:rgba(0,0,0,0.9);">Applicant</p>
          <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid rgba(0,0,0,0.08);border-radius:8px;">
            <tr>
              <td style="padding:4px 18px;">
                <table width="100%" cellpadding="0" cellspacing="0">
                  {{applicantRows}}
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      {{organizationBlock}}

      <!-- Project details -->
      <tr>
        <td style="padding:20px 32px 0;">
          <p style="margin:0 0 10px;font-size:14px;font-weight:600;color:rgba(0,0,0,0.9);">Project details</p>
          <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid rgba(0,0,0,0.08);border-radius:8px;">
            <tr>
              <td style="padding:4px 18px;">
                <table width="100%" cellpadding="0" cellspacing="0">
                  {{Row("Project name", dto.Project_Name)}}
                  {{Row("Project ID", dto.Project_Academic_ID)}}
                  
                
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      {{motivationBlock}}

      <!-- CTA -->
      <tr>
        <td style="padding:28px 32px 8px;">
          <table cellpadding="0" cellspacing="0">
            <tr>
              <td style="padding-right:10px;">
                <a href="" style="display:inline-block;padding:10px 22px;background:{{accentColor}};color:#ffffff;font-size:14px;font-weight:600;text-decoration:none;border-radius:20px;">Review request</a>
              </td>
              <td>
                <a href="" style="display:inline-block;padding:10px 22px;background:#ffffff;color:rgba(0,0,0,0.9);font-size:14px;font-weight:600;text-decoration:none;border-radius:20px;border:1px solid rgba(0,0,0,0.6);">View project</a>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <!-- Sign-off -->
      <tr>
        <td style="padding:20px 32px 28px;">
          <p style="margin:0;font-size:14px;color:rgba(0,0,0,0.75);">Regards,<br><span style="color:rgba(0,0,0,0.9);font-weight:600;">The Alumni Portal Team</span></p>
        </td>
      </tr>

      <!-- Footer -->
      <tr>
        <td style="background:#f4f2ee;padding:16px 32px;border-top:1px solid rgba(0,0,0,0.08);">
          <p style="margin:0;font-size:12px;color:rgba(0,0,0,0.6);text-align:center;">This is an automated notification from Alumni Portal. Please do not reply to this email.</p>
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
