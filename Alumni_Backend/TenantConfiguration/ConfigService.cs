using Alumni_Portal.TenantConfiguration.Parameter;
using Alumni_Portal.TenantConfiguration.Parameter.DTO;

namespace Alumni_Portal.TenantConfiguration
{
    public class ConfigService
    {
        private ProjectParameters _projectParameters;
        private IndividualParameters _individualParameters;
        private PostParameters _postParameters;
        public ConfigService(ProjectParameters projectParameters, IndividualParameters individualParameters, PostParameters postParameters)
        {
            _projectParameters = projectParameters;
            _individualParameters = individualParameters;
            _postParameters = postParameters;
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

        public async Task<PostDirectoryParams> GetPostMetadata()
        {
            var metadata=await _postParameters.GetPostDirectoryParameters();
            return metadata;
        }
    }
}
