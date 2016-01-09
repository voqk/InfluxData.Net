﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using InfluxData.Net.InfluxDb.Models;
using InfluxData.Net.Common.Helpers;
using InfluxData.Net.InfluxDb.Infrastructure;

namespace InfluxData.Net.Integration.Tests
{
    [Collection("Integration")]
    [Trait("Integration", "Diagnostics")]
    public class IntegrationDiagnostics : IDisposable
    {
        private readonly IntegrationFixture _fixture;

        public IntegrationDiagnostics(IntegrationFixture fixture)
        {
            _fixture = fixture;
            _fixture.TestSetup();
        }

        public void Dispose()
        {
            _fixture.TestTearDown();
        }

        [Fact]
        public async Task ClientPing_ShouldReturnVersion()
        {
            var pong = await _fixture.Sut.Diagnostics.PingAsync();

            pong.Should().NotBeNull();
            pong.Success.Should().BeTrue();
            pong.Version.Should().NotBeEmpty();
        }
    }
}