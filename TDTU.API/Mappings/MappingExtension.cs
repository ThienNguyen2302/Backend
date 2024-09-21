﻿using AutoMapper.QueryableExtensions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace TDTU.API.Mappings;

public static class MappingExtensions
{
	public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
		=> PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

	public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
		=> queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();


	public static PaginatedList<TDestination> PaginatedListNoAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
	   => PaginatedList<TDestination>.Create(queryable.AsNoTracking(), pageNumber, pageSize);


}