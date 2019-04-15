// @material-ui/icons
import Dashboard from "@material-ui/icons/Dashboard";
//import Person sfrom "@material-ui/icons/Person";
//import LibraryBooks from "@material-ui/icons/LibraryBooks";
import BubbleChart from "@material-ui/icons/BubbleChart";
//import LocationOn from "@material-ui/icons/LocationOn";
import Notifications from "@material-ui/icons/Notifications";
//import Unarchive from "@material-ui/icons/Unarchive";
//import Language from "@material-ui/icons/Language";
// core components/views for Admin layout
import DashboardPage from "views/Dashboard/Dashboard.jsx";


//import UserProfile from "views/UserProfile/UserProfile.jsx";
import TableList from "views/TableList/TableList.jsx";
//import Typography from "views/Typography/Typography.jsx";
//import Icons from "views/Icons/Icons.jsx";
//import Maps from "views/Maps/Maps.jsx";
//import NotificationsPage from "views/Notifications/Notifications.jsx";
//import UpgradeToPro from "views/UpgradeToPro/UpgradeToPro.jsx";
// core components/views for RTL layout
import RTLPage from "views/RTLPage/RTLPage.jsx";

import JobCostSummaryPage from "views/JobCostSummary/JobCostSummary.js";
import PurchaseOrderPage from "views/PurchaseOrder/PurchaseOrder.jsx";
import VariancePurchaseOrderPage from "views/VariancePurchaseOrder/VariancePurchaseOrder.jsx";

const dashboardRoutes = [
  {
    path: "/dashboard",
    name: "Dashboard",
    rtlName: "لوحة القيادة",
    icon: Dashboard,
    component: DashboardPage,
    layout: "/admin"
  },
  {
    path: "/jobcostsummary",
    name: "Job Cost Summary",
    rtlName: "قائمة الجدول",
    icon: "content_paste",
    component: JobCostSummaryPage,
    layout: "/admin"
    },
    {
        path: "/purchaseorder",
        name: "Purchase Order",
        rtlName: "إخطارات",
        icon: BubbleChart,
        component: PurchaseOrderPage,
        layout: "/admin"
    },
    {
        path: "/variancepurchaseorder",
        name: "VPO",
        rtlName: "إخطارات",
        icon: Notifications,
        component: VariancePurchaseOrderPage,
        layout: "/admin"
    },
];

export default dashboardRoutes;
