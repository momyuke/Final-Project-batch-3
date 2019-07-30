﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.DTO;
using FinalProject.Filters;

namespace FinalProject.Utils
{
    [AuthFilter]
    public class MenuUtils
    {

//------------------------------------------------------------ method for get menu based on role id of user login --------------------------------
        public List<MenuDTO> GetMenus()
        {
            using(DBEntities db = new DBEntities())
            {
                //get data user in user dto format by session user login
                var Context = HttpContext.Current;
                UserDTO UserLogin = (UserDTO)Context.Session["UserLogin"];

                //get data menus from table menu and change to menudto
                //get data menu from tb_menu joined with tb_access menu and where tb_access_menu.role id equal userlogin.role_id
                List<MenuDTO> ListMenu = (from Menu in db.TB_MENU
                                         join AccessMenu in db.TB_ACCESS_MENU
                                         on Menu.MENU_ID equals AccessMenu.MENU_ID
                                         where AccessMenu.ROLE_ID.Equals(UserLogin.ROLE_ID)
                                         select new MenuDTO
                                         {
                                             MENU_ID = Menu.MENU_ID,
                                             TITLE_MENU = Menu.TITLE_MENU,
                                             LOGO_MENU = Menu.LOGO_MENU,
                                         }).ToList();
                return ListMenu;
            }

        }

//----------------------------------------------method for get sub menu based on menu id is based on role id user login-----------------------------
        public List<SubMenuDTO> GetSubmenu(MenuDTO Menu)
        {
            using(DBEntities db = new DBEntities())
            {
                List<SubMenuDTO> ListSubMenuDTO = (from SubMenu in db.TB_SUBMENU where SubMenu.MENU_ID.Equals(Menu.MENU_ID)
                                                   select new SubMenuDTO{
                                                       SUB_MENU_ID = SubMenu.SUB_MENU_ID,
                                                       MENU_ID = SubMenu.MENU_ID,
                                                       TITLE_SUBMENU = SubMenu.TITLE_SUBMENU,
                                                       LOGO_SUBMENU = SubMenu.LOGO_SUBMENU,
                                                       URL = SubMenu.URL
                                                   } ).ToList();
                return ListSubMenuDTO;
            }
        }
 //---------------------------------------------------method for check access menu base menu id and role id ----------------------------
    
        public static string CheckAccess(int RoleId, int MenuId)
        {
            string res = null;

            using(DBEntities db  = new DBEntities())
            {
                TB_ACCESS_MENU Tb_Access_Menu = db.TB_ACCESS_MENU.FirstOrDefault(ac => ac.MENU_ID == MenuId && ac.ROLE_ID == RoleId);

                if(Tb_Access_Menu != null)
                {
                    res = "checked='checked'";
                }

                return res;
            }
        }
//---------------------------------------------------method for check access menu candidate---------------------------------------------
        public static string CheckAccessCandidate(int RoleId, int SubMenuCandidateId, int ActionCandidateId)
        {
            string res = null;

            using(DBEntities db = new DBEntities())
            {
                TB_USER_ACCESS_MENU_CANDIDATE TbUserAccessMenuCandidate =
                    db.TB_USER_ACCESS_MENU_CANDIDATE.FirstOrDefault(ac =>
                        ac.ROLE_ID == RoleId &&
                        ac.SUB_MENU_CANDIDATE_ID == SubMenuCandidateId &&
                        ac.ACTION_CANDIDATE_ID == ActionCandidateId);
                if(TbUserAccessMenuCandidate != null)
                {
                    res = "checked=checked";
                }
            
                return res;
            }
        }

    //---------------------------------------------------------------- check access button action user ----------------------------------------------
        public static string CheckButtonAcc(int ActionId,int SubMenuId)
        {
            string res = "visible";

            //get data user login
            UserDTO UserLogin = (UserDTO)HttpContext.Current.Session["UserLogin"];

            //get data from tb acess action candidate
            using(DBEntities db = new DBEntities())
            {
                var Acc = db.TB_USER_ACCESS_MENU_CANDIDATE.FirstOrDefault(d => d.ROLE_ID == UserLogin.ROLE_ID && d.ACTION_CANDIDATE_ID == ActionId && d.SUB_MENU_CANDIDATE_ID == SubMenuId);
                if(Acc == null)
                {
                    res = "invisible";
                }
            }

            return res;

        }
    }
}