using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class FacebookRepository
    {
        #region FacebookLogin
        public static ZResponse FacebookLogin(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookLoginRequest>(data);
            return XHelper.Invoke("POST", "Facebook/FacebookLogin", req);
        }
        #endregion

        #region Connect
        public static ZResponse GetPageConnectByShopID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookConnectGetByIdRequest>(data);
            return XHelper.Invoke("GET", "Facebook/GetPageConnectByShopID?FbId=" + req.FBID + "&ShopID=" + req.ShopID, null);
        }
        public static ZResponse AddConnectPage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAddConnectRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddConnectPage", req);
        }
        public static ZResponse AddConnectPage_V2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAddConnectRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddConnectPage_V2", req);
        }
        public static ZResponse DeleteLinkConnectPage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookDeleteConnectRequest>(data);
            return XHelper.Invoke("POST", "Facebook/DeleteLinkConnectPage", req);
        }
        #endregion

        #region Conversation assign
        public static ZResponse ListConversationAssign(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchConversationAssignRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ListConversationAssign", req);
        }
        public static ZResponse AddConversationAssign(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAssignAddRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddConversationAssign", req);
        }
        public static ZResponse EditConversationAssign(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAssignEditRequest>(data);
            return XHelper.Invoke("POST", "Facebook/EditConversationAssign", req);
        }
        public static ZResponse DeleteConversationAssign(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAssignDeleteRequest>(data);
            return XHelper.Invoke("POST", "Facebook/DeleteConversationAssign", req);
        }
        #endregion

        #region Feed
        public static ZResponse ListFeed(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchFeedRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ListFeed", req);
        }
        #endregion

        #region Conversation Detail
        public static ZResponse ListComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchCommentRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ListComment", req);
        }
        public static ZResponse AddAssignToConversation(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookConverAssignToConversationRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddAssignToConversation", req);
        }
        public static ZResponse DeleteAssign(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookDeleteAssignModelRequest>(data);
            return XHelper.Invoke("POST", "Facebook/DeleteAssign", req);
        }
        #endregion

        #region Seen - Not Seen
        public static ZResponse MaskAsViewed(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookMaskAsViewedRequest>(data);
            return XHelper.Invoke("POST", "Facebook/MaskAsViewed", req);
        }
        public static ZResponse MaskAsRead(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookMaskAsReadRequest>(data);
            return XHelper.Invoke("POST", "Facebook/MaskAsRead", req);
        }
        public static ZResponse MaskAsViewedDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookMaskAsViewedRequest>(data);
            return XHelper.Invoke("POST", "Facebook/MaskAsViewedDetail", req);
        }
        public static ZResponse MaskAsReadDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookMaskAsReadRequest>(data);
            return XHelper.Invoke("POST", "Facebook/MaskAsReadDetail", req);
        }
        #endregion

        #region Check info Customer
        public static ZResponse CheckCustomerInfo(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookCheckCustomerInfoRequestData>(data);
            return XHelper.Invoke("POST", "Facebook/CheckCustomerInfo", req);
        }
        #endregion

        #region Split Conversation Employee
        public static ZResponse SplitConversation(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSplitConversationRequest>(data);
            return XHelper.Invoke("POST", "Facebook/SplitConversation", req);
        }
        public static ZResponse CountConversationSplit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookCountConversationSplitRequest>(data);
            return XHelper.Invoke("POST", "Facebook/CountConversationSplit", req);
        }
        public static ZResponse SplitConversationMulti(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSplitConversationRequest>(data);
            return XHelper.Invoke("POST", "Facebook/SplitConversationMulti", req);
        }
        #endregion

        #region AutoLike Comment    
        public static ZResponse PageAutoLike(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchPageAutoLikeRequest>(data);
            return XHelper.Invoke("POST", "Facebook/PageAutoLike", req);
        }
        public static ZResponse ChangeStatusPageAutoLike(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookReactionPageAutoLikeRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ChangeStatusPageAutoLike", req);
        }
        #endregion

        #region Auto Hidden Comment
        public static ZResponse PageAutoHidden(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchPageAutoHiddenRequest>(data);
            return XHelper.Invoke("POST", "Facebook/PageAutoHidden", req);
        }
        public static ZResponse EditPageHidden(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookEditPageAutoHiddenRequest>(data);
            return XHelper.Invoke("POST", "Facebook/EditPageAutoHidden", req);
        }
        public static ZResponse ChangeStatusPageAutoHidden(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookChangeStatusPageAutoHiddenRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ChangeStatusPageAutoHidden", req);
        }
        public static ZResponse GetPageAutoHiddenByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetAutoHiddenByIDRequest>(data);
            return XHelper.Invoke("POST", "Facebook/GetPageAutoHiddenByID", req);
        }
        #endregion

        #region Working Time
        public static ZResponse EditWorkingTime(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookEditWorkingTimeRequest>(data);
            return XHelper.Invoke("POST", "Facebook/EditWorkingTime", req);
        }
        public static ZResponse DeleteWorkingTime(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookDeleteWorkingTimeRequest>(data);
            return XHelper.Invoke("POST", "Facebook/DeleteWorkingTime", req);
        }
        public static ZResponse ListWorkingTime(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchWorkingTimeRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ListWorkingTime", req);
        }
        public static ZResponse GetWorkingTimeByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetWorkingTimeByIDRequest>(data);
            return XHelper.Invoke("POST", "Facebook/GetWorkingTimeByID", req);
        }
        #endregion

        #region Working Time Detail
        public static ZResponse AddWorkingTimeDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAddWorkingTimeDetailRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddWorkingTimeDetail", req);
        }
        public static ZResponse EditWorkingTimeDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookEditWorkingTimeDetailRequest>(data);
            return XHelper.Invoke("POST", "Facebook/EditWorkingTimeDetail", req);
        }
        public static ZResponse DeleteWorkingTimeDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookDeleteWorkingTimeDetailRequest>(data);
            return XHelper.Invoke("POST", "Facebook/DeleteWorkingTimeDetail", req);
        }
        public static ZResponse WorkingTimeDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchWorkingTimeDetailRequest>(data);
            return XHelper.Invoke("POST", "Facebook/WorkingTimeDetail", req);
        }
        #endregion

        #region Send Mass Messages
        public static ZResponse AddMessagesCampaign(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAddMessageCampaignRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddMessagesCampaign", req);
        }
        public static ZResponse ListMessagesCampaign(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookListMessageCampaignRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ListMessagesCampaign", req);
        }
        public static ZResponse AddSendMassMessages(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAddSendMassMessagesRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddSendMassMessages", req);
        }
        public static ZResponse ListSendMassMessages(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchSendMassMessagesRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ListSendMassMessages", req);
        }
        #endregion

        #region FeedbackAuto
        public static ZResponse AddPageFeedbackAuto(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAddFeedbackAutoRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddPageFeedbackAuto", req);
        }
        public static ZResponse ListFeedbackAuto(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSearchFeedbackAutoRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ListFeedbackAuto", req);
        }
        public static ZResponse ChangeFeedbackAuto(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookUpdateFeedbackAutoRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ChangeFeedbackAuto", req);
        }
        public static ZResponse DeleteFeedbackAuto(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookUpdateFeedbackAutoRequest>(data);
            return XHelper.Invoke("POST", "Facebook/DeleteFeedbackAuto", req);
        }
        #endregion

        #region Add XML Conversation - Conversation Detail
        public static ZResponse AddXMLConversation(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAddConversationXMLRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddXMLConversation", req);
        }
        public static ZResponse AddXMLConversationDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookAddConversationDetailXMLRequest>(data);
            return XHelper.Invoke("POST", "Facebook/AddXMLConversationDetail", req);
        }
        #endregion

        #region ResetConnectPage
        public static ZResponse ResetConnectPage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookResetConnectPageRequest>(data);
            return XHelper.Invoke("POST", "Facebook/ResetConnectPage", req);
        }
        #endregion

        
    }
}