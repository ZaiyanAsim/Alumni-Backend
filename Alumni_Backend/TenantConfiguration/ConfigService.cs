using Alumni_Portal.TenantConfiguration.Parameter;
using Alumni_Portal.TenantConfiguration.Parameter.DTO;

namespace Alumni_Portal.TenantConfiguration
{
    public class ConfigService
    {
        private ProjectParameters _projectParameters;
        private IndividualParameters _individualParameters;
        public ConfigService(ProjectParameters projectParameters, IndividualParameters individualParameters)
        {
            _projectParameters = projectParameters;
            _individualParameters = individualParameters;
        }


        public async Task<ProjectDirectoryParams> GetProjectMetadata()
        {
            return await _projectParameters.GetProjectDirectoryParameters();
        }

        public async Task<UserDirectoryParams> GetIndividualMetadata()
        {
            return await _individualParameters.GetUserDirectoryParameters();
        }

        public async Task<List<Parameter_Base_String_DTO>?> ProjectCreateMetadata()
        {
            var metadata = await _individualParameters.GetSupervisorsAsync();
            return metadata;
        }
    }
}
// Do you really think that you need abstractions