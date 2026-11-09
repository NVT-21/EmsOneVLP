using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Library
{
    public class CommandCodeConst
    {
        #region EMPLOYEE
        public const string EMPLOYEE_LOGIN = "EMPLOYEE_LOGIN";
        public const string EMPLOYEE_COUNT_ACCOUNT = "EMPLOYEE_COUNT_ACCOUNT";
        public const string EMPLOYEE_LIST_ACCOUNT = "EMPLOYEE_LIST_ACCOUNT";
        public const string EMPLOYEE_ADD_NEW = "EMPLOYEE_ADD_NEW";
        public const string EMPLOYEE_SHOP_ADD = "EMPLOYEE_SHOP_ADD";
        public const string EMPLOYEE_LIST = "EMPLOYEE_LIST";
        public const string EMPLOYEE_EDIT = "EMPLOYEE_EDIT";
        public const string EMPLOYEE_SHOP_EDIT = "EMPLOYEE_SHOP_EDIT";
        public const string EMPLOYEE_GET_BY_ID = "EMPLOYEE_GET_BY_ID";
        public const string EMPLOYEE_GET_BY_GROUP_PERMISSION_ID = "EMPLOYEE_GET_BY_GROUP_PERMISSION_ID";
        public const string EMPLOYEE_CHANGE_IS_LOCK = "EMPLOYEE_CHANGE_IS_LOCK";
        public const string EMPLOYEE_RESET_PASSWORD = "EMPLOYEE_RESET_PASSWORD";
        public const string EMPLOYEE_CHANGE_PASSWORD = "EMPLOYEE_CHANGE_PASSWORD";
        public const string EMPLOYEE_REMOVE = "EMPLOYEE_REMOVE";
        public const string EMPLOYEE_TIME_ACTIVE_ADD = "EMPLOYEE_TIME_ACTIVE_ADD";
        public const string EMPLOYEE_TIME_ACTIVE_EDIT = "EMPLOYEE_TIME_ACTIVE_EDIT";
        public const string EMPLOYEE_TIME_ACTIVE_GET_BY_EMPLOYEE_ID = "EMPLOYEE_TIME_ACTIVE_GET_BY_EMPLOYEE_ID";
        public const string EMPLOYEE_CHECK_ACTIVE = "EMPLOYEE_CHECK_ACTIVE";
        public const string EMPLOYEE_CHECK_CONDITION = "EMPLOYEE_CHECK_CONDITION";
        #endregion

        #region CUSTOMER
        public const string CUSTOMER_ADDNEW = "CUSTOMER_ADDNEW";
        public const string CUSTOMER_LIST = "CUSTOMER_LIST";
        public const string CUSTOMER_EDIT = "CUSTOMER_EDIT";
        public const string GET_BY_CUSTOMER_CODE = "GET_BY_CUSTOMER_CODE";
        public const string GET_BY_MOBILE_NUMBER = "GET_BY_MOBILE_NUMBER";
        public const string ADDNEW_GROUP = "ADDNEW_GROUP";
        public const string SEARCH_GROUP = "SEARCH_GROUP";
        public const string CUSTOMER_GET_BY_ID = "CUSTOMER_GET_BY_ID";
        public const string CUSTOMER_CHECK_BLACK_LIST = "CUSTOMER_CHECK_BLACK_LIST";
        public const string CUSTOMER_ADD_TEMP = "CUSTOMER_ADD_TEMP";
        public const string CUSTOMER_ADD_COMMIT = "CUSTOMER_ADD_COMMIT";
        public const string CUSTOMER_CHECK_ID_BY_MOBILE_NUMBER = "CUSTOMER_CHECK_ID_BY_MOBILE_NUMBER";
        public const string CUSTOMER_STICKER_LIST = "CUSTOMER_STICKER_LIST";
        #endregion

        #region WAREHOUSE
        public const string WAREHOUSE_ADDNEW = "WAREHOUSE_ADDNEW";
        public const string WAREHOUSE_LIST = "WAREHOUSE_LIST";
        public const string WAREHOUSE_LIST_PRODUCT_BY_WAREHOUSE_ID = "WAREHOUSE_LIST_PRODUCT_BY_WAREHOUSE_ID";
        public const string WAREHOUSE_GET_BY_ID = "WAREHOUSE_GET_BY_ID";
        public const string WAREHOUSE_LIST_PRODUCT = "WAREHOUSE_LIST_PRODUCT";
        public const string WAREHOUSE_ADD_INVENTORY = "WAREHOUSE_ADD_INVENTORY";
        public const string WAREHOUSE_EDIT = "WAREHOUSE_EDIT";
        public const string WAREHOUSE_GET_PRODUCTS_BY_ID = "WAREHOUSE_GET_PRODUCTS_BY_ID";
        public const string WAREHOUSE_IS_DEFAULT = "WAREHOUSE_IS_DEFAULT";
        public const string WAREHOUSE_GET_DEFAULT_WAREHOUSE = "WAREHOUSE_GET_DEFAULT_WAREHOUSE";
        public const string WAREHOUSE_CHANGE_IS_LOCK = "WAREHOUSE_CHANGE_IS_LOCK";
        #endregion

        #region WAREHOUSE_PRODUCT
        public const string WAREHOUSE_PRODUCT_UPDATE = "WAREHOUSE_PRODUCT_UPDATE";
        public const string WAREHOUSE_PRODUCT_GET_BY_VARIANT_ID = "WAREHOUSE_PRODUCT_GET_BY_VARIANT_ID";
        #endregion

        #region CORPORATION
        public const string CORPORATION_ADDNEW = "CORPORATION_ADDNEW";
        public const string CORPORATION_LIST = "CORPORATION_LIST";
        public const string CORPORATION_GET_BY_ID = "CORPORATION_GET_BY_ID";
        public const string CORPORATION_EDIT = "CORPORATION_EDIT";
        public const string CORPORATION_REMOVE = "CORPORATION_REMOVE";
        public const string CORPORATION_GET_LIST_EMPLOYEE_BY_CORPORATION_ID = "CORPORATION_GET_LIST_EMPLOYEE_BY_CORPORATION_ID";
        public const string CORPORATION_REMOVE_EMPLOYEE = "CORPORATION_REMOVE_EMPLOYEE";
        #endregion

        #region SHOP
        public const string SHOP_GET_BY_ID = "SHOP_GET_BY_ID";
        public const string SHOP_GET_HISTORY_BY_ID = "SHOP_GET_HISTORY_BY_ID";
        public const string SHOP_SEARCH_SHOP = "SHOP_SEARCH_SHOP";
        public const string SHOP_REGISTER = "SHOP_REGISTER";
        public const string SHOP_LOGIN = "SHOP_LOGIN";
        public const string SHOP_APPROVED_SHOP = "SHOP_APPROVED_SHOP";
        public const string SHOP_REJECT_SHOP = "SHOP_REJECT_SHOP";
        public const string SHOP_UPDATE_CONFIRMATION_EMAIL = "SHOP_UPDATE_CONFIRMATION_EMAIL";
        public const string SHOP_CHANGE_IS_LOCK = "SHOP_CHANGE_IS_LOCK";
        public const string SHOP_RESET_PASSWORD_SHOP = "SHOP_RESET_PASSWORD_SHOP";
        public const string SHOP_EDIT_SHOP = "SHOP_EDIT_SHOP";
        public const string SHOP_EDIT = "SHOP_EDIT";
        public const string SHOP_SALES_CHANNEL_LIST = "SHOP_SALES_CHANNEL_LIST";
        public const string SHOP_SALES_CHANNEL_ADD = "SHOP_SALES_CHANNEL_ADD";
        public const string SHOP_SALES_CHANNEL_REMOVE = "SHOP_SALES_CHANNEL_REMOVE";
        public const string SHOP_GET_PICKUP_INFORMATION = "SHOP_GET_PICKUP_INFORMATION";
        public const string SHOP_CHANGE_PICK_UP_ADDRESS = "SHOP_CHANGE_PICK_UP_ADDRESS";
        public const string SHOP_GROUP_PERMISSION_LIST = "SHOP_GROUP_PERMISSION_LIST";
        public const string SHOP_GROUP_PERMISSION_CHANGE_IS_LOCK = "SHOP_GROUP_PERMISSION_CHANGE_IS_LOCK";
        public const string SHOP_GROUP_PERMISSION_ADD = "SHOP_GROUP_PERMISSION_ADD";
        public const string SHOP_GROUP_PERMISSION_GET_BY_ID = "SHOP_GROUP_PERMISSION_GET_BY_ID";
        public const string SHOP_GROUP_PERMISSION_EDIT = "SHOP_GROUP_PERMISSION_EDIT";
        public const string SHOP_LEVEL_GET_BY_PARENT_ID = "SHOP_LEVEL_GET_BY_PARENT_ID";
        public const string SHOP_LEVEL_EDIT = "SHOP_LEVEL_EDIT";
        public const string SHOP_LEVEL_ADD = "SHOP_LEVEL_ADD";
        public const string SHOP_TRANSPORT_SERVICE_BY_SHOP_ID = "SHOP_TRANSPORT_SERVICE_BY_SHOP_ID";
        public const string SHOP_REMOVE = "SHOP_REMOVE";
        public const string SHOP_LOCK = "SHOP_LOCK";
        public const string SHOP_GET_LIST_CRM_OR_PP_CODE = "SHOP_GET_LIST_CRM_OR_PP_CODE";
        public const string SHOP_FORGET_PASSWORD_BY_EMAIL = "SHOP_FORGET_PASSWORD_BY_EMAIL";
        public const string SHOP_FORGET_PASSWORD_UPDATE = "SHOP_FORGET_PASSWORD_UPDATE";
        #endregion

        #region PRODUCT
        public const string PRODUCT_ADD = "PRODUCT_ADD";
        public const string PRODUCT_GET_BY_ID = "PRODUCT_GET_BY_ID";
        public const string PRODUCT_EDIT = "PRODUCT_EDIT";
        public const string PRODUCT_LIST = "PRODUCT_LIST";
        public const string PRODUCT_REMOVE = "PRODUCT_REMOVE";
        public const string PRODUCT_LIST_IMAGE = "PRODUCT_LIST_IMAGE";
        public const string PRODUCT_REMOVE_IMAGE = "PRODUCT_REMOVE_IMAGE";
        public const string PRODUCT_ADD_IMAGES = "PRODUCT_ADD_IMAGES";
        public const string PRODUCT_UPDATE_AVATAR = "PRODUCT_UPDATE_AVATAR";
        public const string PRODUCT_GET_VARIANT_BY_ID = "PRODUCT_GET_VARIANT_BY_ID";
        public const string PRODUCT_GET_BY_VARIANT_ID = "PRODUCT_GET_BY_VARIANT_ID";
        public const string PRODUCT_DATA_IMAGES_BY_ID = "PRODUCT_DATA_IMAGES_BY_ID";
        public const string PRODUCT_EDIT_IMAGES = "PRODUCT_EDIT_IMAGES";
        #endregion

        #region PRODUCT_VARIANT
        public const string PRODUCT_VARIANT_GET_BY_ID = "PRODUCT_VARIANT_GET_BY_ID";
        public const string PRODUCT_VARIANT_GET_BY_IDS = "PRODUCT_VARIANT_GET_BY_IDS";
        public const string PRODUCT_VARIANT_GET_BY_SKU = "PRODUCT_VARIANT_GET_BY_SKU";
        public const string PRODUCT_VARIANT_GET_DETAIL_BY_ID = "PRODUCT_VARIANT_GET_DETAIL_BY_ID";
        public const string PRODUCT_VARIANT_GET_BY_SHOP_ID = "PRODUCT_VARIANT_GET_BY_SHOP_ID";
        public const string PRODUCT_VARIANT_UPDATE_IMAGE = "PRODUCT_VARIANT_UPDATE_IMAGE";
        public const string PRODUCT_VARIANT_ADD = "PRODUCT_VARIANT_ADD";
        public const string PRODUCT_VARIANT_EDIT = "PRODUCT_VARIANT_EDIT";
        public const string PRODUCT_VARIANT_REMOVE = "PRODUCT_VARIANT_REMOVE";
        public const string PRODUCT_VARIANT_GET_BY_PRODUCT_ID = "PRODUCT_VARIANT_GET_BY_PRODUCT_ID";
        #endregion

        #region PRODUCT_OPTION
        public const string PRODUCT_OPTION_GET_BY_PRODUCT_ID = "PRODUCT_OPTION_GET_BY_PRODUCT_ID";
        public const string PRODUCT_OPTION_ADD = "PRODUCT_OPTION_ADD";
        public const string PRODUCT_OPTION_REMOVE_BY_NAME = "PRODUCT_OPTION_REMOVE_BY_NAME";
        #endregion

        #region PRODUCT_GROUP
        public const string PRODUCT_GROUP_ADD = "PRODUCT_GROUP_ADD";
        public const string PRODUCT_GROUP_GET_BY_ID = "PRODUCT_GROUP_GET_BY_ID";
        public const string PRODUCT_GROUP_EDIT = "PRODUCT_GROUP_EDIT";
        public const string PRODUCT_GROUP_LIST = "PRODUCT_GROUP_LIST";
        #endregion

        #region PRODUCT_UNIT
        public const string PRODUCT_UNIT_SEARCH_LIST = "PRODUCT_UNIT_SEARCH_LIST";
        public const string PRODUCT_UNIT_ADD = "PRODUCT_UNIT_ADD";
        #endregion

        #region EMPLOYEE_LEVEL
        public const string EMPLOYEE_LEVEL_GETS = "EMPLOYEE_LEVEL_GETS";
        #endregion

        #region PROVIDER
        public const string PROVIDER_LIST = "PROVIDER_LIST";
        public const string PROVIDER_ADD = "PROVIDER_ADD";
        public const string PROVIDER_GET_BY_ID = "PROVIDER_GET_BY_ID";
        public const string PROVIDER_EDIT = "PROVIDER_EDIT";
        public const string PROVIDER_CHANGE_IS_LOCK = "PROVIDER_CHANGE_IS_LOCK";
        #endregion

        #region PROMOTION
        public const string PROMOTION_ADD_NEW_CODE = "PROMOTION_ADD_NEW_CODE";
        public const string PROMOTION_ADD_NEW = "PROMOTION_ADD_NEW";
        public const string PROMOTION_GET_BY_CODE = "PROMOTION_GET_BY_CODE";
        public const string PROMOTION_LIST = "PROMOTION_LIST";
        public const string PROMOTION_IS_LOCK = "PROMOTION_IS_LOCK";
        public const string PROMOTION_REMOVE = "PROMOTION_REMOVE";
        #endregion

        #region PROVINCES, DISTRICT, WARD
        public const string PROVINCE_LIST = "PROVINCE_LIST";
        public const string PROVINCE_GET_PRO_ID_BY_PRO_NAME = "PROVINCE_GET_PRO_ID_BY_PRO_NAME";
        public const string DISTRICT_GET_BY_PROVINCE_ID = "DISTRICT_GET_BY_PROVINCE_ID";
        public const string DISTRICT_GET_DIS_ID_BY_DIS_NAME = "DISTRICT_GET_DIS_ID_BY_DIS_NAME";
        public const string WARD_GETS = "WARD_GETS";
        public const string WARD_GET_BY_DISTRICT_ID = "WARD_GET_BY_DISTRICT_ID";
        public const string WARD_GET_WARD_ID_BY_WARD_NAME = "WARD_GET_WARD_ID_BY_WARD_NAME";
        #endregion

        #region FUNCTIONAL
        public const string FUNCTIONAL_LIST = "FUNCTIONAL_LIST";
        #endregion

        #region EXCEL
        public const string IMPORT_EXCEL_ADD = "IMPORT_EXCEL_ADD";
        #endregion

        #region OPTION
        public const string OPTION_GET_DATA_BY_SHOP_ID = "OPTION_GET_DATA_BY_SHOP_ID";
        public const string OPTION_GET_DATA_BY_PARENT_ID = "OPTION_GET_DATA_BY_PARENT_ID";
        public const string OPTION_GET_PARENT_BY_PRODUCT_ID = "OPTION_GET_PARENT_BY_PRODUCT_ID";
        #endregion

        #region ORDER
        public const string ORDER_SEARCH_PRODUCT = "ORDER_SEARCH_PRODUCT";
        public const string ORDER_ADD = "ORDER_ADD";
        public const string ORDER_ADD_AND_SEND_TRANSPORT = "ORDER_ADD_AND_SEND_TRANSPORT";
        public const string ORDER_SEND_TRANSPORT = "ORDER_SEND_TRANSPORT";
        public const string ORDER_CANCEL_TRANSPORT = "ORDER_CANCEL_TRANSPORT";
        public const string ORDER_SEARCH = "ORDER_SEARCH";
        //public const string ORDER_CHANGE_STATUS = "ORDER_CHANGE_STATUS";
        public const string ORDER_GET_BY_ID = "ORDER_GET_BY_ID";
        //public const string ORDER_SEARCH_INFO_SEND_REQUEST_TRANSPORT = "ORDER_SEARCH_INFO_SEND_REQUEST_TRANSPORT";
        public const string ORDER_GET_BY_SHIPPING_CODE = "ORDER_GET_BY_SHIPPING_CODE";
        public const string ORDER_DETAIL_BY_SHIPPING_CODE = "ORDER_DETAIL_BY_SHIPPING_CODE";
        #endregion

        #region REQUEST_ORDER
        public const string REQUEST_ORDER_LIST = "REQUEST_ORDER_LIST";
        public const string REQUEST_ORDER_GET_BY_ID = "REQUEST_ORDER_GET_BY_ID";
        public const string REQUEST_ORDER_ADD_TEMP = "REQUEST_ORDER_ADD_TEMP";
        public const string REQUEST_ORDER_ADD_COMMIT = "REQUEST_ORDER_ADD_COMMIT";
        public const string REQUEST_ORDER_ADD = "REQUEST_ORDER_ADD";
        public const string ADDNEW_REQUEST_ACCEPT = "ADDNEW_REQUEST_ACCEPT";
        public const string REQUEST_ORDER_LIST_DIVISION = "REQUEST_ORDER_LIST_DIVISION";
        public const string REQUEST_ORDER_EXCEL_DIVISION = "REQUEST_ORDER_EXCEL_DIVISION";
        public const string REQUEST_ORDER_CHANGE_STATUS = "REQUEST_ORDER_CHANGE_STATUS";
        public const string REQUEST_ORDER_CHANGE_REASON = "REQUEST_ORDER_CHANGE_REASON";
        public const string REQUEST_ORDER_LIST_DIVISION_AGAIN = "REQUEST_ORDER_LIST_DIVISION_AGAIN";
        #endregion

        #region REQUEST_SUPPORT
        public const string REQUEST_SUPPORT_LIST = "REQUEST_SUPPORT_LIST";
        public const string REQUEST_SUPPORT_ADD = "REQUEST_SUPPORT_ADD";
        public const string REQUEST_SUPPORT_SEARCH_INFO_SEND_REQUEST_SUPPORT = "REQUEST_SUPPORT_SEARCH_INFO_SEND_REQUEST_SUPPORT";
        #endregion

        #region REQUEST_SUPPORT_GROUP
        public const string REQUEST_SUPPORT_GROUP_LIST = "REQUEST_SUPPORT_GROUP_LIST";
        #endregion

        #region FACEBOOK

        #endregion

        #region FACEBOOK_SERVICES
        #endregion

        #region DELIVERY_NOTE
        public const string DELIVERY_NOTE_LIST = "DELIVERY_NOTE_LIST";
        public const string DELIVERY_NOTE_GET_BY_ID = "DELIVERY_NOTE_GET_BY_ID";
        public const string DELIVERY_NOTE_GET_BY_ORDER_ID = "DELIVERY_NOTE_GET_BY_ORDER_ID";
        public const string DELIVERY_NOTE_ADD = "DELIVERY_NOTE_ADD";
        public const string DELIVERY_NOTE_CHANGE_STATUS = "DELIVERY_NOTE_CHANGE_STATUS";
        public const string DELIVERY_NOTE_COMPLETED_STATUS = "DELIVERY_NOTE_COMPLETED_STATUS";
        #endregion

        #region STATUS_PROCESS
        public const string STATUS_PROCESS_LIST = "STATUS_PROCESS_LIST";
        public const string STATUS_PROCESS_ADD = "STATUS_PROCESS_ADD";
        public const string STATUS_PROCESS_CHANGE_IS_LOCK = "STATUS_PROCESS_CHANGE_IS_LOCK";
        public const string STATUS_PROCESS_REMOVE = "STATUS_PROCESS_REMOVE";
        #endregion

        #region PERMISSION
        public const string PERMISSION_LIST = "PERMISSION_LIST";
        public const string PERMISSION_LIST_FUNCTIONAL_GROUP = "PERMISSION_LIST_FUNCTIONAL_GROUP";
        public const string PERMISSION_CHANGE = "PERMISSION_CHANGE";
        #endregion

        #region FE_TRANSPORT
        public const string SEARCH_LIST_NEWSWITHORDER = "SEARCH_LIST_NEWSWITHORDER";
        public const string SEARCH_INFO_SHOP = "SEARCH_INFO_SHOP";
        public const string SEARCH_INFO_SHOP_ORDER_DETAIL = "SEARCH_INFO_SHOP_ORDER_DETAIL";
        public const string UPDATE_STATUS_TRANSPORT_ORDER = "UPDATE_STATUS_TRANSPORT_ORDER";
        public const string GET_EMS_BARCODE = "GET_EMS_BARCODE";
        public const string GET_EMS_BARCODE_SHIPPING_CANCEL = "GET_EMS_BARCODE_SHIPPING_CANCEL";
        #endregion

        #region APIONETECH
        public const string GET_ACCESS_TOKEN_ZOHO = "GET_ACCESS_TOKEN_ZOHO";
        public const string GET_ZOHOCRM_LEADS_PHONE = "GET_ZOHOCRM_LEADS_PHONE";
        public const string GET_ZOHOCRM_LEADS_EMAIL = "GET_ZOHOCRM_LEADS_EMAIL";
        public const string ADD_ZOHOCRM_LEADS = "ADD_ZOHOCRM_LEADS";
        public const string CREATE_PICKUP_EMS = "CREATE_PICKUP_EMS";
        public const string CANCEL_PICKUP_EMS = "CANCEL_PICKUP_EMS";
        #endregion

        #region STATISTICAL
        public const string STATISTICAL_TRANSPORT = "STATISTICAL_TRANSPORT";
        public const string STATISTICAL_ORDER = "STATISTICAL_ORDER";
        public const string STATISTICAL_CUSTOMER = "STATISTICAL_CUSTOMER";
        public const string STATISTICAL_CASHFLOW = "STATISTICAL_CASHFLOW";
        public const string STATISTICAL_REQUEST_ORDER = "STATISTICAL_REQUEST_ORDER";
        #endregion

        #region TRANSPORT_SERVICES
        public const string TRANSPORT_SERVICE_LIST = "TRANSPORT_SERVICE_LIST";
        public const string TRANSPORT_SERVICE_ADD = "TRANSPORT_SERVICE_ADD";
        #endregion

        #region TRANSPORT_POSTAGE
        public const string TRANSPORT_POSTAGE_CHARGED = "TRANSPORT_POSTAGE_CHARGED";
        #endregion

        #region TRANSPORT_EMS
        public const string TRANSPORT_EMS_BARCODE_GET = "TRANSPORT_EMS_BARCODE_GET";
        #endregion

        #region TRANSPORT_ORDER
        public const string TRANSPORT_ORDER_UPDATE_STATUS = "TRANSPORT_ORDER_UPDATE_STATUS";
        #endregion

        #region CMS
        public const string CMS_ADD_REQUEST_SUPPORT = "CMS_ADD_REQUEST_SUPPORT";
        #endregion

        #region INVENTORY
        public const string INVENTORY_CONFIRM = "INVENTORY_CONFIRM";
        #endregion

        #region ZOHO
        public const string ZOHO_ACCESS_TOKEN_GET = "ZOHO_ACCESS_TOKEN_GET"; 
        public const string ZOHO_LEADS_PHONE_GET = "ZOHO_LEADS_PHONE_GET"; 
        public const string ZOHO_LEADS_EMAIL_GET = "ZOHO_LEADS_EMAIL_GET"; 
        public const string ZOHO_LEADS_ADD = "ZOHO_LEADS_ADD";
        #endregion

        #region PNS
        public const string PNS_EMS_PICKUP_CREATE = "PNS_EMS_PICKUP_CREATE";
        #endregion

        #region COLLECTION_POST_OFFICE
        public const string COLLECTION_POST_OFFICE_SEARCH_LIST = "COLLECTION_POST_OFFICE_SEARCH_LIST";
        public const string COLLECTION_POST_OFFICE_SEARCH_LIST_RECENT = "COLLECTION_POST_OFFICE_SEARCH_LIST_RECENT";
        #endregion

        #region OTP
        public const string OTP_VERIFY_CODE = "OTP_VERIFY_CODE";
        #endregion

        #region ORDER_BATCH
        public const string ORDER_BATCH_ADD = "ORDER_BATCH_ADD";
        public const string ORDER_BATCH_EDIT_INFO = "ORDER_BATCH_EDIT_INFO";
        public const string ORDER_BATCH_EDIT = "ORDER_BATCH_EDIT";
        public const string ORDER_BATCH_COPPY_INFO = "ORDER_BATCH_COPPY_INFO";
        public const string ORDER_BATCH_DETAIL_BY_BATCH_CODE = "ORDER_BATCH_DETAIL_BY_BATCH_CODE";
        #endregion

        #region EXPECTED_DELIVERY
        public const string EXPECTED_DELIVERY_SEARCH_DATE = "EXPECTED_DELIVERY_SEARCH_DATE";
        #endregion
    }
}
