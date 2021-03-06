﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxData.Net.InfluxDb.Constants;
using InfluxData.Net.InfluxDb.Enums;
using InfluxData.Net.InfluxDb.Infrastructure;
using InfluxData.Net.InfluxDb.Models;

namespace InfluxData.Net.InfluxDb.RequestClients.Modules
{
    internal class CqRequestModule : RequestModuleBase, ICqRequestModule
    {
        public CqRequestModule(IInfluxDbRequestClient requestClient) 
            : base(requestClient)
        {
        }

        public async Task<IInfluxDbApiResponse> CreateContinuousQuery(ContinuousQuery continuousQuery)
        {
            var downsamplers = BuildDownsamplers(continuousQuery.Downsamplers);
            var tags = BuildTags(continuousQuery.Tags);
            var fillType = BuildFillType(continuousQuery.FillType);

            var subQuery = String.Format(QueryStatements.CreateContinuousQuerySubQuery, 
                downsamplers, continuousQuery.DsSerieName, continuousQuery.SourceSerieName, continuousQuery.Interval, tags, fillType);

            var query = String.Format(QueryStatements.CreateContinuousQuery, continuousQuery.CqName, continuousQuery.DbName, subQuery);

            return await this.RequestClient.GetQueryAsync(RequestClientUtility.BuildQueryRequestParams(continuousQuery.DbName, query));
        }

        public async Task<IInfluxDbApiResponse> GetContinuousQueries(string dbName)
        {
            return await this.RequestClient.GetQueryAsync(RequestClientUtility.BuildQueryRequestParams(dbName, QueryStatements.GetContinuousQueries));
        }

        public async Task<IInfluxDbApiResponse> DeleteContinuousQuery(string dbName, string cqName)
        {
            var query = String.Format(QueryStatements.DropContinuousQuery, cqName, dbName);
            return await this.RequestClient.GetQueryAsync(RequestClientUtility.BuildQueryRequestParams(dbName, query));
        }

        public async Task<IInfluxDbApiResponse> Backfill(string dbName, Backfill backfill)
        {
            var downsamplers = BuildDownsamplers(backfill.Downsamplers);
            var filters = BuildFilters(backfill.Filters);
            var timeFrom = backfill.TimeFrom.ToString("yyyy-MM-dd HH:mm:ss");
            var timeTo = backfill.TimeTo.ToString("yyyy-MM-dd HH:mm:ss");
            var tags = BuildTags(backfill.Tags);
            var fillType = BuildFillType(backfill.FillType);

            var query = String.Format(QueryStatements.Backfill, 
                downsamplers, backfill.DsSerieName, backfill.SourceSerieName, filters, timeFrom, timeTo, backfill.Interval, tags, fillType);

            return await this.RequestClient.GetQueryAsync(RequestClientUtility.BuildQueryRequestParams(dbName, query));
        }

        private static string BuildDownsamplers(IList<string> downsamplers)
        {
            return String.Join(", ", downsamplers);
        }

        private static string BuildFilters(IList<string> filters)
        {
            return filters == null ? String.Empty : String.Join(" ", String.Join("AND ", filters), "AND");
        }

        private static string BuildTags(IList<string> tags)
        {
            return tags == null ? String.Empty : String.Join(" ", ",", String.Join(", ", tags));
        }

        private static string BuildFillType(FillType fillType)
        {
            return fillType == FillType.Null ? String.Empty : String.Format(QueryStatements.Fill, fillType.ToString().ToLower());
        }
    }
}
