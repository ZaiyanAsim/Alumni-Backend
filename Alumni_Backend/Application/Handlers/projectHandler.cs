using Microsoft.EntityFrameworkCore;
using System.Linq;
using Admin.Application.Services;
using Admin.Infrastructure.Repositories;
using Admin.Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Admin.Application.Abstractions;

namespace Admin.Application.Handlers;

public class projectHandler
{
    private IProjectService _projectService;

    public projectHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }


    //public async Task<List<projectDTO>> getProjectsPaginated(List<int>types,List<int>Categories, int _page, int _limit)
    //{
    //    try
    //    {
    //        var projects = await _projectService.getProjects(type)
    //         .Skip((_page - 1) * _limit)
    //         .Take(_limit)
    //         .ToListAsync();


    //        return projects;
    //    }
    //    catch (Exception)
    //    {
    //        throw new Exception("Error occured while fetching Projects table");
    //    }
    //}

    //public async Task<int> createUser(NewUserDTO newUser)
    //{
    //    try
    //    {
    //        int id = await _Service.create(newUser);

    //        return id;



    //    }

    //    catch (Exception ex)
    //    {
    //        throw new Exception("Error creating user: " + ex.Message);
    //    }
    //}











}
