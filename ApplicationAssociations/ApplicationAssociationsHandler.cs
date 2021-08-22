using System.Collections.Generic;
using System.Drawing;

namespace ApplicationAssociations
{
    public class VerbLink
    {
        public string Verb { get; set; }
        public string Command { get; set; }

        public VerbLink(string verb, string command)
        {
            Verb = verb;
            Command = command;
        }

        public override string ToString()
        {
            return Verb.ToString();
        }
    }

    public class ApplicationData
    {
        public string ProgId { get; set; }
        public string FriendlyAppName { get; set; }
        public string ApplicationId { get; set; }
        public string Command { get; set; }
        public string AppIconReference { get; set; }

        private Icon loadedIcon = null;
        public Icon Icon { 
            get
            {
                if (loadedIcon != null) return loadedIcon;
                return loadedIcon = ApplicationIcon.GetApplicationIcon(AppIconReference);
                //if (appIconReference.StartsWith("@{")) appIconReference = ExtractNormalIconPath(appIconReference);
            }
            set { loadedIcon = value; }
        }

        public List<VerbLink> VerbLinks { get; } = new List<VerbLink>(); 
        public void AddVerb (string verb, string command)
        {            
            foreach (VerbLink verbLink in VerbLinks)
            {
                if (verbLink.Verb == verb) return; //Already there
            }
            VerbLinks.Add(new VerbLink(verb, command));
        }

        public override string ToString()
        {
            return FriendlyAppName.ToString();
        }
    }


    public class ApplicationAssociationsHandler
    {
        private Dictionary<string, List<ApplicationData>> extentionApplications = new Dictionary<string, List<ApplicationData>>();

        private void AddApplicationData(string extention, ApplicationData applicationData)
        {
            if (!extentionApplications.ContainsKey(extention)) extentionApplications.Add(extention, new List<ApplicationData>());
            foreach (ApplicationData applicationDataSearch in extentionApplications[extention])
            {
                if (applicationDataSearch.FriendlyAppName == applicationData.FriendlyAppName)
                    return;
                if (applicationDataSearch.ProgId == applicationData.ProgId)
                    return;

                //if (applicationDataSearch.ApplicationId == applicationData.ApplicationId)
                //    return;
            }
            extentionApplications[extention].Add(applicationData);
        }

        

        public void Add(string extention)
        {

            List<string> progIds = ProtocolAssociationsRegistry.GetProgIdExtentionAssociations(extention); 

            foreach (string progId in progIds)
            {
                var verbs = ProtocolAssociationsRegistry.GetVerbsByProgId(progId);

                if (verbs != null && verbs.Length > 0)
                {
                    ApplicationData applicationData = new ApplicationData();
                    applicationData.ProgId = progId;

                    AssocQuery assocQuery = new AssocQuery(progId);
                    applicationData.ApplicationId = assocQuery.AppId;
                    applicationData.Command = assocQuery.Command;
                    applicationData.FriendlyAppName = assocQuery.FriendlyAppName;
                    applicationData.AppIconReference = assocQuery.AppIconReference;
                    AddApplicationData(extention, applicationData);

                    foreach (string verb in verbs)
                    {
                        string command = "";
                        if (!string.IsNullOrEmpty(applicationData.Command))
                        {
                            command = AssocQuery.GetCommandProgId(progId, verb);                            
                        }
                        applicationData.AddVerb(verb, command);
                    }
                    AddApplicationData(extention, applicationData);
                }
            }
        }

        public List<ApplicationData> OpenWithInCommon(List<string> extensions)
        {
            List<ApplicationData> commonExtentionApplications = null;
            foreach (string extention in extensions)
            {
                Add(extention);
            }

            foreach (string extention in extensions)
            {
                if (commonExtentionApplications == null) //Build a common list to start with
                {
                    commonExtentionApplications = new List<ApplicationData>();

                    foreach (ApplicationData applicationData in extentionApplications[extention])
                    {
                        commonExtentionApplications.Add(applicationData);
                    }
                }
                else //Removed from common list to that is not common for all
                {
                    List<ApplicationData> listOpenWithInBothList = new List<ApplicationData>();
                    foreach (ApplicationData applicationData in extentionApplications[extention])
                    {                        
                        //Check exist in common both list
                        foreach (ApplicationData checkIfExist in commonExtentionApplications)
                        {
                            if (checkIfExist.FriendlyAppName == applicationData.FriendlyAppName)
                            {
                                listOpenWithInBothList.Add(applicationData);
                                break;
                            }
                        }
                        
                    }
                    commonExtentionApplications = listOpenWithInBothList;
                }
            }
            return commonExtentionApplications;
        }

        public List<VerbLink> GetVerbLinks(string progId)
        {
            foreach (KeyValuePair< string, List<ApplicationData>> keyValuePair in extentionApplications)
            {
                List<ApplicationData> applicationDatas = keyValuePair.Value; 
                foreach (ApplicationData applicationDataSearch in applicationDatas)
                {
                    if (applicationDataSearch.ProgId == progId) return applicationDataSearch.VerbLinks;
                }
            }
            return null;
        }

        public VerbLink GetVerbLink(string progId, string verb)
        {
            foreach (KeyValuePair<string, List<ApplicationData>> keyValuePair in extentionApplications)
            {
                List<ApplicationData> applicationDatas = keyValuePair.Value;
                foreach (ApplicationData applicationDataSearch in applicationDatas)
                {
                    if (applicationDataSearch.ProgId == progId)
                    {
                        foreach (VerbLink verbLink in applicationDataSearch.VerbLinks)
                        {
                            if (verbLink.Verb.Equals(verb, System.StringComparison.InvariantCultureIgnoreCase)) return verbLink;
                        }
                    }
                }
            }
            return null;
        }
    }
}
