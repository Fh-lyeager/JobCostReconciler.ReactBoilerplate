import React from "react";
import PropTypes from "prop-types";
// react plugin for creating charts
import ChartistGraph from "react-chartist";
// @material-ui/core
import withStyles from "@material-ui/core/styles/withStyles";
import Icon from "@material-ui/core/Icon";
// @material-ui/icons
import Store from "@material-ui/icons/Store";
import Warning from "@material-ui/icons/Warning";
import DateRange from "@material-ui/icons/DateRange";
import LocalOffer from "@material-ui/icons/LocalOffer";
import Update from "@material-ui/icons/Update";
import ArrowUpward from "@material-ui/icons/ArrowUpward";
import AccessTime from "@material-ui/icons/AccessTime";
import Accessibility from "@material-ui/icons/Accessibility";
import BugReport from "@material-ui/icons/BugReport";
import Code from "@material-ui/icons/Code";
import Cloud from "@material-ui/icons/Cloud";
// core components
import GridItem from "components/Grid/GridItem.jsx";
import GridContainer from "components/Grid/GridContainer.jsx";
import Table from "components/Table/Table.jsx";
import Tasks from "components/Tasks/Tasks.jsx";
import CustomTabs from "components/CustomTabs/CustomTabs.jsx";
import Danger from "components/Typography/Danger.jsx";
import Card from "components/Card/Card.jsx";
import CardHeader from "components/Card/CardHeader.jsx";
import CardIcon from "components/Card/CardIcon.jsx";
import CardBody from "components/Card/CardBody.jsx";
import CardFooter from "components/Card/CardFooter.jsx";

import { bugs, website, server } from "variables/general.jsx";
import {
  dailySalesChart,
  emailsSubscriptionChart,
  completedTasksChart
} from "variables/charts.jsx";
import { purchaseOrderProcessorChart } from "variables/pocharts.jsx";

import dashboardStyle from "assets/jss/material-dashboard-react/views/dashboardStyle.jsx";

class PurchaseOrderPage extends React.Component {
  state = {
    value: 0,
    purchaseOrderLastRun: [],
    purchaseOrderNextRun: [],
    purchaseOrderNextRuns: [],
    purchaseOrderQueueItems: [],
      itemsInQueue: [],
      failedRecords: []
  };
  handleChange = (event, value) => {
    this.setState({ value });
  };

  handleChangeIndex = index => {
    this.setState({ value: index });
  };

  componentDidMount() {
    this.getPurchaseOrderLastRun();
    this.getPurchaseOrderNextRun();
    this.getPurchaseOrderLastRuns();
    this.getItemsInQueue();
    this.getFailedRecords();
  }

  getPurchaseOrderLastRuns() {
    fetch('api/PurchaseOrderQueue')
      .then(response => response.json())
      .then(data => {
        this.setState({ purchaseOrderNextRuns: data });
      });
  }

    getPurchaseOrderNextRun() {
        fetch('api/PurchaseOrderQueue/NextRun')
            .then(response => response.json())
            .then(data => {
                this.setState({ purchaseOrderNextRun: Array.from(data) });
            });
    }

    getPurchaseOrderLastRun() {
        fetch('api/PurchaseOrderQueue/LastRun')
            .then(response => response.json())
            .then(data => {
                this.setState({ purchaseOrderLastRun: Array.from(data) });
            });
    }

    getItemsInQueue() {
        fetch('api/PurchaseOrderQueue/ItemsInQueue')
            .then(response => response.json())
            .then(data => {
                this.setState({ itemsInQueue: Array.from(data) });
            });
    }

    getFailedRecords() {
        fetch('api/PurchaseOrderQueue/FailedRecords')
            .then(response => response.json())
            .then(data => {
                this.setState({ failedRecords: Array.from(data) });
            });
    }

  static renderPurchaseOrderLastRuns(purchaseOrderNextRuns, pageNumber = 1) {
    var pageSize = 8;
    var nextRunData = purchaseOrderNextRuns.slice((pageNumber - 1) * pageSize, pageNumber * pageSize);

    return (
      <tbody>
        {nextRunData.map(item =>
          <tr key={item.nextRunId}>
            <td>{item.nextRunId}</td>
            <td>{item.runComplete}</td>
            <td>{item.status}</td>
          </tr>
        )}
      </tbody>
      )
  }


    render() {
      let classes = this.props;
      let runs = !(this.state.purchaseOrderRuns === undefined) && this.state.purchaseOrderNextRuns.length < 1 ? [] : PurchaseOrderPage.renderPurchaseOrderLastRuns(this.state.purchaseOrderNextRuns);
      let purchaseOrderNextRun = this.state.purchaseOrderNextRun.length < 1 ? "" : this.state.purchaseOrderNextRun;

      let purchaseOrderQueue = this.state.purchaseOrderQueueItems.length < 1 ? [] : this.state.purchaseOrderQueueItems;


    return (
      <div>
        <GridContainer>
          <GridItem xs={12} sm={6} md={4}>
            <Card>
              <CardHeader color="warning" stats icon>
                <CardIcon color="warning">
                  <Icon>content_copy</Icon>
                </CardIcon>
                <p className={classes.cardCategory}>Next Run</p>
                <h3 className={classes.cardTitle}>
                  {purchaseOrderNextRun}
                </h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <DateRange />
                  <a href="#pablo" onClick={e => e.preventDefault()}>
                    Last run {this.state.purchaseOrderLastRun}
                  </a>
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={6} md={4}>
            <Card>
              <CardHeader color="success" stats icon>
                <CardIcon color="success">
                  <Store />
                </CardIcon>
                <p className={classes.cardCategory}>Items In Queue</p>
                <h3 className={classes.cardTitle}>{this.state.itemsInQueue}</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <Update />
                  Process Queue
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={6} md={4}>
            <Card>
              <CardHeader color="danger" stats icon>
                <CardIcon color="danger">
                  <Icon>info_outline</Icon>
                </CardIcon>
                <p className={classes.cardCategory}>Errors</p>
                <h3 className={classes.cardTitle}>
                    {this.state.failedRecords}
                </h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <LocalOffer />
                  View
                </div>
              </CardFooter>
            </Card>
          </GridItem>
        </GridContainer>
        <GridContainer>
          <GridItem xs={12} sm={12} md={4}>
            <Card chart>
              <CardHeader color="success">
                <ChartistGraph
                  className="ct-chart"
                  data={dailySalesChart.data}
                  type="Line"
                  options={dailySalesChart.options}
                  listener={dailySalesChart.animation}
                />
              </CardHeader>
              <CardBody>
                <h4 className={classes.cardTitle}>Purchase Order Processor</h4>
                <p className={classes.cardCategory}>
                  
                </p>
              </CardBody>
              <CardFooter chart>
                <div className={classes.stats}>
                  <AccessTime /> updated 4 minutes ago
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={12} md={4}>
            <Card chart>
              <CardHeader color="warning">
                <ChartistGraph
                  className="ct-chart"
                  data={emailsSubscriptionChart.data}
                  type="Bar"
                  options={emailsSubscriptionChart.options}
                  responsiveOptions={emailsSubscriptionChart.responsiveOptions}
                  listener={emailsSubscriptionChart.animation}
                />
              </CardHeader>
              <CardBody>
                <h4 className={classes.cardTitle}>Exceptions</h4>
                <p className={classes.cardCategory}>
                  Last Campaign Performance
                </p>
              </CardBody>
              <CardFooter chart>
                <div className={classes.stats}>
                  <AccessTime /> campaign sent 2 days ago
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={12} md={4}>
            <Card chart>
              <CardHeader color="danger">
                <ChartistGraph
                  className="ct-chart"
                  data={completedTasksChart.data}
                  type="Line"
                  options={completedTasksChart.options}
                  listener={completedTasksChart.animation}
                />
              </CardHeader>
              <CardBody>
                <h4 className={classes.cardTitle}>Completed Tasks</h4>
                <p className={classes.cardCategory}>
                  Last Campaign Performance
                </p>
              </CardBody>
              <CardFooter chart>
                <div className={classes.stats}>
                  <AccessTime /> campaign sent 2 days ago
                </div>
              </CardFooter>
            </Card>
          </GridItem>
        </GridContainer>
        <GridContainer>
          <GridItem xs={12} sm={12} md={6}>
            <CustomTabs
              title="Tasks:"
              headerColor="primary"
              tabs={[
                {
                  tabName: "Bugs",
                  tabIcon: BugReport,
                  tabContent: (
                    <Tasks
                      checkedIndexes={[0, 3]}
                      tasksIndexes={[0, 1, 2, 3]}
                      tasks={bugs}
                    />
                  )
                },
                {
                  tabName: "Website",
                  tabIcon: Code,
                  tabContent: (
                    <Tasks
                      checkedIndexes={[0]}
                      tasksIndexes={[0, 1]}
                      tasks={website}
                    />
                  )
                },
                {
                  tabName: "Server",
                  tabIcon: Cloud,
                  tabContent: (
                    <Tasks
                      checkedIndexes={[1]}
                      tasksIndexes={[0, 1, 2]}
                      tasks={server}
                    />
                  )
                }
              ]}
            />
          </GridItem>
          <GridItem xs={12} sm={12} md={6}>
            <Card>
              <CardHeader color="warning">
                <h4 className={classes.cardTitleWhite}>Completed Runs</h4>
                <p className={classes.cardCategoryWhite}>
                  New employees on 15th September, 2016
                </p>
              </CardHeader>
              <CardBody>
                <table width="100%">
                  <thead>
                    <tr>
                      <th align="left">ID</th>
                      <th align="left">Date</th>
                      <th align="left">Status</th>
                    </tr>
                  </thead>
                  {runs}
                </table>
              </CardBody>
            </Card>
          </GridItem>
        </GridContainer>
      </div>
    );
  }
}

PurchaseOrderPage.propTypes = {
    classes: PropTypes.object.isRequired
};

//var lastRunTime = <div>{PurchaseOrderPage.purchaseOrderLastRun}</div>

export default withStyles(dashboardStyle)(PurchaseOrderPage);
