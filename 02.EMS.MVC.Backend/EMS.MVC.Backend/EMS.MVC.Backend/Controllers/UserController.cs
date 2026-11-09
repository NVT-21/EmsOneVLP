using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Linq;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
namespace EMS.MVC.Backend.Controllers
{
    public class UserController : BaseController
    {
        #region Danh sách tài khoản vận hành

        [AuthorizeController]
        public ActionResult UserView()
        {
            UserProfile session = new UserProfile();
            if(XHelper.SessionUser != null)
            {
                session = (UserProfile)Session[XHelper.SessionUser];
            }

            // Kiểm tra quyền, nếu không phải admin thì return null view
            if(session.GroupCode != ".0.0.")
            {
                return new HttpStatusCodeResult(403, "Bạn không có quyền truy cập."); ; // hoặc return new EmptyResult(); tùy theo yêu cầu
            }


            UserViewModel model = new UserViewModel()
            {
                ListUserGroup = XCommon.SearchListUserGroup(new ZUserGroupSearchListRequest() { IsLock = "N" })
            };
            return View(model);
        }

        [AuthorizeController]
        public ActionResult SearchListUserView(UserViewModel req)
        {
            UserProfile session = new UserProfile();
            if(XHelper.SessionUser != null)
            {
                session = (UserProfile)Session[XHelper.SessionUser];
            }

            // Kiểm tra quyền, nếu không phải admin thì return null view
            if(session.GroupCode != ".0.0.")
            {
                return new HttpStatusCodeResult(403, "Bạn không có quyền truy cập."); ; // hoặc return new EmptyResult(); tùy theo yêu cầu
            }

            List<SearchListUserViewModel> model = new List<SearchListUserViewModel>();
            var res = UserRepository.SearchList(new ZUserSearchListRequest()
            {
                UserID = XHelper.Session.UserProfile.ID,
                UserGroupCode = req.UserGroupCode,
                Email = req.Email,
                MobileNumber = req.MobileNumber
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZUserSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new SearchListUserViewModel()
                    {
                        ID = item.ID,
                        UserName = item.UserName,
                        UserGroup = new UserGroupModel()
                        {
                            Code = item.GroupCode,
                            Name = item.GroupName
                        },
                        FullName = item.FullName,
                        MobileNumber = item.MobileNumber,
                        Email = item.Email,
                        IsLock = item.IsLock,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        LastLogin = item.LastLogin
                    });
                }
            }
            return PartialView(model);
        }

        #endregion

        #region Thêm tài khoản vận hành
        [AuthorizeController]
        public ActionResult AddUserView()
        {
            UserProfile session = new UserProfile();
            if(XHelper.SessionUser != null)
            {
                session = (UserProfile)Session[XHelper.SessionUser];
            }

            // Kiểm tra quyền, nếu không phải admin thì return null view
            if(session.GroupCode != ".0.0.")
            {
                return new HttpStatusCodeResult(403, "Bạn không có quyền truy cập."); ; // hoặc return new EmptyResult(); tùy theo yêu cầu
            }

            AddUserViewModel model = new AddUserViewModel()
            {
                ListUserGroup = XCommon.SearchListUserGroup(new ZUserGroupSearchListRequest() { IsLock = "N" }),
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult JAddUser(AddUserViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = UserRepository.AddUser(new ZUserAddRequest()
                {
                    UserName = req.UserName,
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    Email = req.Email,
                    GroupCode = req.UserGroupCode,
                    UserCMS = req.UserCMS,
                    ServesCustomer = req.ServesCustomer,
                    ListCollectionPostOfficeID = req.ListCollectionPostOfficeID,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.JAddUser: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JAddCollectionPostOfficeOfUser()
        {
            List<TreeModel> tree = new List<TreeModel>();
            try
            {
                //var res = UserInCollectionPostOfficeRepository.SearchList(new ZUserInCollectionPostOfficeSearchListRequest());
                var res = UserInCollectionPostOfficeRepository.SearchListByUserID(new ZUserInCollectionPostOfficeSearchListByUserIDRequest()
                {
                    UserID = 0,
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZUserInCollectionPostOfficeSearchListByUserIDResponse>>(res.Data);
                    tree.Add(new TreeModel() {
                        id = "0",
                        text = "Tất cả",
                        parent = "#",
                        state = new TreeStateModel()
                        {
                            selected = true,
                        },
                    });
                    foreach (var item in list)
                    {
                        var CollectionPostOfficeName = item.CollectionPostOfficeName;

                        if (item.ParentID > 0)
                        {
                            CollectionPostOfficeName = item.CollectionPostOfficeCode + " - " + item.CollectionPostOfficeName;
                        }

                        if (item.ParentID == 0 && item.NumberOfChild == 0)
                        {
                            continue;
                        }

                        tree.Add(new TreeModel()
                        {
                            id = item.ID.ToString(),
                            text = CollectionPostOfficeName,
                            parent = item.ParentID == 0 ? "0" : item.ParentID.ToString(),
                            state = new TreeStateModel()
                            {
                                selected = item.IsChoose == "Y" ? true : false,
                            },
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.JSearchListCollectionPostOfficeTree: " + ex.Message);
            }
            return Json(tree, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa tài khoản vận hành
        [AuthorizeController]
        public ActionResult EditUserView(SearchListUserViewModel req)
        {
            EditUserViewModel model = null;
            var UserID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
            var res = UserRepository.GetByID(new ZUserGetByIDRequest()
            {
                UserID = UserID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZUserGetByIDResponse>(res.Data);
                model = new EditUserViewModel()
                {
                    ID = item.ID,
                    UserName = item.UserName,
                    UserGroupCode = item.GroupCode,
                    ListUserGroup = XCommon.SearchListUserGroup(new ZUserGroupSearchListRequest() { IsLock = "N" }),
                    FullName = item.FullName,
                    MobileNumber = item.MobileNumber,
                    Email = item.Email,
                    ServesCustomer = item.ServesCustommer,
                    UserCMS = item.UserCMS,
                };
            }
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult JEditUser(EditUserViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = UserRepository.EditUser(new ZUserEditRequest()
                {
                    ID = req.ID,
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    Email = req.Email,
                    GroupCode = req.UserGroupCode,
                    ServesCustomer = req.ServesCustomer,
                    UserCMS = req.UserCMS,
                    ListCollectionPostOfficeID =req.ListCollectionPostOfficeID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.JEditUser: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JEditCollectionPostOfficeOfUser(int UserID)
        {
            List<TreeModel> tree = new List<TreeModel>();
            try
            {
                var res = UserInCollectionPostOfficeRepository.SearchListByUserID(new ZUserInCollectionPostOfficeSearchListByUserIDRequest()
                {
                    UserID = UserID,
                });

                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZUserInCollectionPostOfficeSearchListByUserIDResponse>>(res.Data);
                    tree.Add(new TreeModel()
                    {
                        id = "0",
                        text = "Tất cả",
                        parent = "#",
                        state = new TreeStateModel()
                        {
                            selected = false,
                        },
                    });
                    foreach (var item in list)
                    {
                        var CollectionPostOfficeName = item.CollectionPostOfficeName;

                        if (item.ParentID > 0)
                        {
                            CollectionPostOfficeName = item.CollectionPostOfficeCode + " - " + item.CollectionPostOfficeName;
                        }

                        if(item.ParentID == 0 && item.NumberOfChild == 0)
                        {
                            continue;
                        }

                        tree.Add(new TreeModel()
                        {
                            id = item.ID.ToString(),
                            text = CollectionPostOfficeName,
                            parent = item.ParentID == 0 ? "0" : item.ParentID.ToString(),
                            state = new TreeStateModel()
                            {
                                selected = item.IsChoose == "Y" ? true : false,
                            },
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.JEditCollectionPostOfficeOfUser: " + ex.Message);
            }
            return Json(tree, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult JChangeToLocked(SearchListUserViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = UserRepository.ChangeToLocked(new ZUserChangeToLockedRequest()
                {
                    ID = req.ID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.JChangeToLocked: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JChangeToUnlocked(SearchListUserViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = UserRepository.ChangeToUnLocked(new ZUserChangeToUnlockedRequest()
                {
                    ID = req.ID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.JChangeToUnlocked: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JResetPassword(int ID, string UserName)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = UserRepository.ResetPassword(new ZUserResetPasswordRequest()
                {
                    ID = ID,
                    UserName = UserName
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.JResetPassword: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //[AuthorizeController]
        public ActionResult ChangePasswordView()
        {
            return PartialView();
        }

        public JsonResult JChangePassword(ChangePasswordViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = UserRepository.ChangePassword(new ZUserChangePasswordRequest()
                {
                    ID = XHelper.Session.UserProfile.ID,
                    UserName = XHelper.Session.UserProfile.UserName,
                    OldPassword = req.OldPassword,
                    NewPassword = req.NewPassword,
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.JChangePassword: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}