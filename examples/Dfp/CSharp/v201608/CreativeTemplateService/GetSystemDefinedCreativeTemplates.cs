// Copyright 2016, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201608;
using Google.Api.Ads.Dfp.v201608;

using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201608 {
  /// <summary>
  /// This example gets all system defined creative templates.
  /// </summary>
  public class GetSystemDefinedCreativeTemplates : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all system defined creative templates.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main() {
      GetSystemDefinedCreativeTemplates codeExample = new GetSystemDefinedCreativeTemplates();
      Console.WriteLine(codeExample.Description);

      codeExample.Run(new DfpUser());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The DFP user object running the code example.</param>
    public void Run(DfpUser user) {
      CreativeTemplateService creativeTemplateService =
          (CreativeTemplateService) user.GetService(DfpService.v201608.CreativeTemplateService);

      // Create a statement to select creative templates.
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("type = :type")
          .OrderBy("id ASC")
          .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
          .AddValue("type", CreativeTemplateType.SYSTEM_DEFINED.ToString());

      // Retrieve a small amount of creative templates at a time, paging through
      // until all creative templates have been retrieved.
      CreativeTemplatePage page = new CreativeTemplatePage();
      try {
        do {
          page = creativeTemplateService.getCreativeTemplatesByStatement(
              statementBuilder.ToStatement());

          if (page.results != null) {
            // Print out some information for each creative template.
            int i = page.startIndex;
            foreach (CreativeTemplate creativeTemplate in page.results) {
              Console.WriteLine("{0}) Creative template with ID \"{1}\" "
                  + "and name \"{2}\" was found.",
                  i++,
                  creativeTemplate.id,
                  creativeTemplate.name);
            }
          }

          statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
        } while (statementBuilder.GetOffset() < page.totalResultSetSize);

        Console.WriteLine("Number of results found: {0}", page.totalResultSetSize);
      } catch (Exception e) {
        Console.WriteLine("Failed to get creative templates. Exception says \"{0}\"",
            e.Message);
      }
    }
  }
}
