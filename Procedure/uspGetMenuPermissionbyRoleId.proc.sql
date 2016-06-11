alter procedure uspGetMenuPermissionbyRoleId @RoleId Varchar(200) as  
begin  
select m.MenuId, m.Name,m.MenuType,ISNULL(mp.Permission,0) Permission,m.ParentMenuId,MP.RoleId from Menu M (NOLOCK)  
LEFT JOIN aspnet_MenuPermission MP (NOLOCK)  
ON M.MenuId =MP.MenuId  
left join aspnet_Role r on r.RoleId = mp.RoleId  
where   r.RoleId is null or MP.RoleId =@RoleId  
  
  
union  
select m.MenuId, m.Name,m.MenuType,ISNULL(mp.Permission,0) Permission,m.ParentMenuId,MP.RoleId from Menu M  
left join aspnet_MenuPermission Mp  
on m.MenuId = mp.MenuId  
and mp.RoleId = @RoleId  
  
  
end  