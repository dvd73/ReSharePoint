using System.Collections.Generic;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public class WebTemplate
        {
            public string Title { get; set; }
            public int Id { get; set; }

            public IEnumerable<WebTemplateConfiguration> Configurations { get; set; }
        }

        public class WebTemplateConfiguration
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public static List<WebTemplate> WebTemplates = new List<WebTemplate>()
        {
            new WebTemplate
            {
                Title = "GLOBAL",
                Id = 0,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Global template",
                        Description = "This template is used for initializing a new site."
                    }
                }
            },
            new WebTemplate
            {
                Title = "STS",
                Id = 1,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Team Site",
                        Description = "A place to work together with a group of people."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 1,
                        Title = "Blank Site",
                        Description = "A blank site for you to customize based on your requirements."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 2,
                        Title = "Document Workspace",
                        Description = "A site for colleagues to work together on a document. It provides a document library for storing the primary document and supporting files, a tasks list for assigning to-do items, and a links list for resources related to the document."
                    }
                }
            },
            new WebTemplate
            {
                Title = "MPS",
                Id = 2,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Basic Meeting Workspace",
                        Description = "A site to plan, organize, and capture the results of a meeting. It provides lists for managing the agenda, meeting attendees, and documents."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 1,
                        Title = "Blank Meeting Workspace",
                        Description = "A blank meeting site for you to customize based on your requirements."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 2,
                        Title = "Decision Meeting Workspace",
                        Description = "A site for meetings that track status or make decisions. It provides lists for creating tasks, storing documents, and recording decisions."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 3,
                        Title = "Social Meeting Workspace",
                        Description = "A site to plan social occasions. It provides lists for tracking attendees, providing directions, and storing pictures of the event."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 4,
                        Title = "Multipage Meeting Workspace",
                        Description = "A site to plan, organize, and capture the results of a meeting. It provides lists for managing the agenda and meeting attendees in addition to two blank pages for you to customize based on your requirements."
                    }
                }
            },
            new WebTemplate
            {
                Title = "CENTRALADMIN",
                Id = 3,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Central Admin Site",
                        Description = "A site for central administration. It provides Web pages and links for application and operations management."
                    }
                }
            },
            new WebTemplate
            {
                Title = "WIKI",
                Id = 4,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Wiki Site",
                        Description = "A site for a community to brainstorm and share ideas. It provides Web pages that can be quickly edited to record information and then linked together through keywords."
                    }
                }
            },
            new WebTemplate
            {
                Title = "BLOG",
                Id = 9,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Blog",
                        Description = "A site for a person or team to post ideas, observations, and expertise that site visitors can comment on."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SGS",
                Id = 15,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Group Work Site",
                        Description = "This template provides a groupware solution that enables teams to create, organize, and share information quickly and easily. It includes Group Calendar, Circulation, Phone-Call Memo, the Document Library and the other basic lists."
                    }
                }
            },
            new WebTemplate
            {
                Title = "TENANTADMIN",
                Id = 16,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Tenant Admin Site",
                        Description = "A site for tenant administration. It provides Web pages and links for self-serve administration."
                    }
                }
            },
            new WebTemplate
            {
                Title = "APP",
                Id = 17,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "App Template",
                        Description = "A base template for app development. It provides the minimal set of features needed for an app."
                    }
                }
            },
            new WebTemplate
            {
                Title = "APPCATALOG",
                Id = 18,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "App Catalog Site",
                        Description = "A site for sharing apps for SharePoint and Office."
                    }
                }
            },
            new WebTemplate
            {
                Title = "ACCSRV",
                Id = 2764,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Access Services Site",
                        Description = "Microsoft Access Server"
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 1,
                        Title = "Assets Web Database",
                        Description = "Create an assets database to keep track of assets, including asset details and owners."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 2,
                        Title = "Charitable Contributions Web Database",
                        Description = "Create a database to track information about fundraising campaigns including donations made by contributors, campaign related events, and pending tasks."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 4,
                        Title = "Contacts Web Database",
                        Description = "Create a contacts database to manage information about people that your team works with, such as customers and partners."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 5,
                        Title = "Projects Web Database",
                        Description = "Create a project tracking database to track multiple projects, and assign tasks to different people."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 6,
                        Title = "Issues Web Database",
                        Description = " Create an issues database to manage a set of issues or problems. You can assign, prioritize, and follow the progress of issues from start to finish. "
                    }
                }
            },
            new WebTemplate
            {
                Title = "BDR",
                Id = 7,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Document Center",
                        Description = "A site to centrally manage documents in your enterprise."
                    }
                }
            },
            new WebTemplate
            {
                Title = "OFFILE",
                Id = 14483,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Records Center (Obsolete)",
                        Description = "This template creates a site designed for records management. Records managers can configure the routing table to direct incoming files to specific locations. The site also lets you manage whether records can be deleted or modified after they are added to the repository."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 1,
                        Title = "Records Center",
                        Description = "This template creates a site designed for records management. Records managers can configure the routing table to direct incoming files to specific locations. The site also lets you manage whether records can be deleted or modified after they are added to the repository."
                    }
                }
            },
            new WebTemplate
            {
                Title = "OSRV",
                Id = 40,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Shared Services Administration Site",
                        Description = "This template creates a site for administering shared services."
                    }
                }
            },
            new WebTemplate
            {
                Title = "PPSMASite",
                Id = 3100,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "PerformancePoint",
                        Description = "A site for presenting PerformancePoint dashboards and scorecards. The site also includes links to PerformancePoint Dashboard Designer and storage for dashboard content such as analytic charts, reports, KPIs, and strategy maps."
                    }
                }
            },
            new WebTemplate
            {
                Title = "BICenterSite",
                Id = 3200,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Business Intelligence Center",
                        Description = "A site for presenting Business Intelligence Center."
                    }
                }
            },
            new WebTemplate
            {
                Title = "PWA",
                Id = 6221,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Project Web App Site"
                    }
                }
            },
            new WebTemplate
            {
                Title = "PWS",
                Id = 6215,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Microsoft Project Site"
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPS",
                Id = 20,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "SharePoint Portal Server Site",
                        Description = "This template is obsolete."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSPERS",
                Id = 21,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "SharePoint Portal Server Personal Space",
                        Description = "This web template defines a Personal Space for an individual participating on a SharePoint Portal."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 2,
                        Title = "Storage And Social SharePoint Portal Server Personal Space",
                        Description = "This web template defines a minimal Personal Space with both Social and Storage features for an individual participating on a SharePoint Portal."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 3,
                        Title = "Storage Only SharePoint Portal Server Personal Space",
                        Description = "This web template defines a minimal Personal Space with Storage features for an individual participating on a SharePoint Portal."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 4,
                        Title = "Social Only SharePoint Portal Server Personal Space",
                        Description = "This web template defines a minimal Personal Space with Social features for an individual participating on a SharePoint Portal."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 5,
                        Title = "Empty SharePoint Portal Server Personal Space",
                        Description = "This web template defines a empty Personal Space."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSMSITE",
                Id = 22,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Personalization Site",
                        Description = "A site for delivering personalized views, data, and navigation from this site collection into My Site. It includes personalization specific Web Parts and navigation that is optimized for My Site sites."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSTOC",
                Id = 30,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Contents area Template",
                        Description = "This template is obsolete."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSTOPIC",
                Id = 31,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Topic area template",
                        Description = "This template is obsolete."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSNEWS",
                Id = 32,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "News Site (Obsolete)",
                        Description = "This template is obsolete."
                    }
                }
            },
            new WebTemplate
            {
                Title = "CMSPUBLISHING",
                Id = 39,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Publishing Site (CMS)",
                        Description = "A blank site for expanding your Web site and quickly publishing Web pages. Contributors can work on draft versions of pages and publish them to make them visible to readers. The site includes document and image libraries for storing Web publishing assets."
                    }
                }
            },
            new WebTemplate
            {
                Title = "BLANKINTERNET",
                Id = 53,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Publishing Site",
                        Description = "This template creates a site for publishing Web pages on a schedule, with workflow features enabled. By default, only Publishing subsites can be created under this site. A Document and Picture Library are included for storing Web publishing assets."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 1,
                        Title = "Press Releases Site",
                        Description = "This template creates the Press Releases subsite for an Internet-facing corporate presence website."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 2,
                        Title = "Publishing Site with Workflow",
                        Description = "A site for publishing Web pages on a schedule by using approval workflows. It includes document and image libraries for storing Web publishing assets. By default, only sites with this template can be created under this site."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSNHOME",
                Id = 33,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "News Site",
                        Description = "A site for publishing news articles and links to news articles. It includes a sample news page and an archive for storing older news items."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSSITES",
                Id = 34,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Site Directory",
                        Description = "A site for listing and categorizing important sites in your organization. It includes different views for categorized sites, top sites, and a site map."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSCOMMU",
                Id = 36,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Community area template",
                        Description = "This template is obsolete."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSREPORTCENTER",
                Id = 38,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Report Center",
                        Description = "A site for creating, managing, and delivering Web pages, dashboards, and key performance indicators that communicate metrics, goals, and business intelligence information."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSPORTAL",
                Id = 47,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Collaboration Portal",
                        Description = "A starter site hierarchy for an intranet divisional portal. It includes a home page, a News site, a Site Directory, a Document Center, and a Search Center with Tabs. Typically, this site has nearly as many contributors as readers and is used to host team sites."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SRCHCEN",
                Id = 50,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Enterprise Search Center",
                        Description = " A site focused on delivering an enterprise-wide search experience. Includes a welcome page with a search box that connects users to four search results page experiences: one for general searches, one for people searches, one for conversation searches, and one for video searches. You can add and customize new results pages to focus on other types of search queries. "
                    }
                }
            },
            new WebTemplate
            {
                Title = "PROFILES",
                Id = 51,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Profiles",
                        Description = "This template creates a profile site that includes page layout with zones."
                    }
                }
            },
            new WebTemplate
            {
                Title = "BLANKINTERNETCONTAINER",
                Id = 52,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Publishing Portal",
                        Description = " A starter site hierarchy for an Internet-facing site or a large intranet portal. This site can be customized easily with distinctive branding. It includes a home page, a sample press releases subsite, a Search Center, and a login page. Typically, this site has many more readers than contributors, and it is used to publish Web pages with approval workflows."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SPSMSITEHOST",
                Id = 54,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "My Site Host",
                        Description = "A site used for hosting personal sites (My Sites) and the public People Profile page. This template needs to be provisioned only once per User Profile Service Application, please consult the documentation for details."
                    }
                }
            },
            new WebTemplate
            {
                Title = "ENTERWIKI",
                Id = 56,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Enterprise Wiki",
                        Description = "A site for publishing knowledge that you capture and want to share across the enterprise. It provides an easy content editing experience in a single location for co-authoring content, discussions, and project management."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SRCHCENTERLITE",
                Id = 90,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Basic Search Center #0",
                        Description = " A site focused on delivering a basic search experience. Includes a welcome page with a search box that connects users to a search results page, and an advanced search page. This Search Center will not appear in navigation."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 1,
                        Title = "Basic Search Center #1",
                        Description = "The Search Center template creates pages dedicated to search. The main welcome page features a simple search box in the center of the page. The template includes a search results and an advanced search page. This Search Center will not appear in navigation."
                    }
                }
            },
            new WebTemplate
            {
                Title = "SRCHCENTERFAST",
                Id = 2000,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "FAST Search Center",
                        Description = "A site for delivering the FAST search experience. The welcome page includes a search box with two tabs: one for general searches, and another for searches for information about people. You can add and customize tabs to focus on other search scopes or result types."
                    }
                }
            },
            new WebTemplate
            {
                Title = "visprus",
                Id = 61,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Visio Process Repository",
                        Description = "A site for teams to quickly view, share, and store Visio process diagrams. It provides a versioned document library for storing process diagrams, and lists for managing announcements, tasks, and review discussions"
                    }
                }
            },
            new WebTemplate
            {
                Title = "EDISC",
                Id = 3300,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "eDiscovery Center",
                        Description = "A site to manage the preservation, search, and export of content for legal matters and investigations."
                    },
                    new WebTemplateConfiguration()
                    {
                        Id = 1,
                        Title = "eDiscovery Case",
                        Description = "This template creates an eDiscovery case. Users create locations where they can preserve or export data."
                    }
                }
            },
            new WebTemplate
            {
                Title = "DOCMARKETPLACESITE",
                Id = 10000,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Academic Library",
                        Description = "The Academic Library template provides a rich view and consumption experience for published content and management. Authors populate metadata and apply rules at the time of publishing, such as description, licensing, and optional rights management (IRM). Visitors of the site can search or browse published titles and add authorized selections to their collection to consume, subject to the rights and rules applied by the author. The site provides an IRM-capable document library, a publishing mechanism for authors to publish documents, detailed views for each document, a check-out mechanism, and related search capabilities."
                    }
                }
            },
            new WebTemplate
            {
                Title = "DEV",
                Id = 95,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Developer Site",
                        Description = "A site for developers to build, test and publish apps for Office"
                    }
                }
            },
            new WebTemplate
            {
                Title = "PROJECTSITE",
                Id = 6115,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Project Site",
                        Description = " A site for managing and collaborating on a project. This site template brings all status, communication, and artifacts relevant to the project into one place."
                    }
                }
            },
            new WebTemplate
            {
                Title = "PRODUCTCATALOG",
                Id = 59,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Product Catalog",
                        Description = " A site for managing product catalog data which can be published to an internet-facing site through search. The product catalog can be configured to support product variants and multilingual product properties. The site includes admin pages for managing faceted navigation for products."
                    }
                }
            },
            new WebTemplate
            {
                Title = "COMMUNITY",
                Id = 62,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Community Site",
                        Description = "A place where community members discuss topics of common interest. Members can browse and discover relevant content by exploring categories, sorting discussions by popularity or by viewing only posts that have a best reply. Members gain reputation points by participating in the community, such as starting discussions and replying to them, liking posts and specifying best replies."
                    }
                }
            },
            new WebTemplate
            {
                Title = "COMMUNITYPORTAL",
                Id = 63,
                Configurations = new[]
                {
                    new WebTemplateConfiguration()
                    {
                        Id = 0,
                        Title = "Community Portal",
                        Description = "A site for discovering communities."
                    }
                }
            }
        };
    }
}
