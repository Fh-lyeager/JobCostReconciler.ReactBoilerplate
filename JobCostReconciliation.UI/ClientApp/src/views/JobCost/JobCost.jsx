import React, { Component } from "react";
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


import WorkflowData from "functions/Workflow.jsx";

import { bugs, website, server } from "variables/general.jsx";

import {
  dailySalesChart,
  emailsSubscriptionChart,
  completedTasksChart
} from "variables/charts.jsx";

import dashboardStyle from "assets/jss/material-dashboard-react/views/dashboardStyle.jsx";

class JobCostPage extends React.Component {
  state = {
      value: 0,
      workflows: [],
      loadingWorkflows: true,
      workflowQueue: [],
      loadingWorkflowQueue: true
  };
  handleChange = (event, value) => {
    this.setState({ value });
  };

  handleChangeIndex = index => {
    this.setState({ value: index });
  };

    componentDidMount() {
        this.getWorkflowData();
        this.getWorkflowQueueItems();

        this.getWorkflowQueueItems("New");
        this.getWorkflowQueueItems("Error");
    }

    getWorkflowData() {
        fetch('api/Workflow')
            .then(response => response.json())
            .then(data => {
                this.setState({ workflows: data, loadingWorkflows: false});
            });
    }

    getWorkflowQueueItems() {

        fetch('api/WorkflowQueue')
            .then(response => response.json())
            .then(data => {
                this.setState({ workflowQueue: data, loadingWorkflowQueue: false });
            });
    }

    filterWorkflowQueue(workflowQueue, status) {

        return workflowQueue.filter((workflowQueue) =>
            status == workflowQueue.status);
    }
    

  render() {
      const { classes } = this.props;
    
      let numberWorkflows = this.state.loadingWorkflows ? "" : this.state.workflows.length;

      let workflowQueueData = this.state.loadingWorkflowQueue ? [] : this.state.workflowQueue;
      let numberWorkflowsProcessed = this.state.loadingWorkflowQueue ? "" : this.state.workflowQueue.length;

      let numberWorkflowsNew = this.state.loadingWorkflowQueue ? [] : this.filterWorkflowQueue(this.state.workflowQueue, "New").length;
      let numberWorkflowsError = this.state.loadingWorkflowQueue ? [] : this.filterWorkflowQueue(this.state.workflowQueue, "Error").length;


    return (
      <div>
        <GridContainer>
          <GridItem xs={12} sm={6} md={3}>
            <Card>
              <CardHeader color="warning" stats icon>
                <CardIcon color="warning">
                  <Icon>content_copy</Icon>
                </CardIcon>
                <p className={classes.cardCategory}>Initiated</p>
                            <h3 className={classes.cardTitle}>
                                {numberWorkflows}
                            </h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <Danger>
                    <Warning />
                  </Danger>
                  <a href="#pablo" onClick={e => e.preventDefault()}>
                    Get more space
                  </a>
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={6} md={3}>
            <Card>
              <CardHeader color="success" stats icon>
                <CardIcon color="success">
                  <Store />
                </CardIcon>
                <p className={classes.cardCategory}>Processed</p>
                            <h3 className={classes.cardTitle}>{numberWorkflowsProcessed}</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <DateRange />
                  Last 24 Hours
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={6} md={3}>
            <Card>
              <CardHeader color="danger" stats icon>
                <CardIcon color="danger">
                  <Icon>info_outline</Icon>
                </CardIcon>
                <p className={classes.cardCategory}>Errors</p>
                <h3 className={classes.cardTitle}>75</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <LocalOffer />
                  Tracked from Github
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={6} md={3}>
            <Card>
              <CardHeader color="info" stats icon>
                <CardIcon color="info">
                  <Accessibility />
                </CardIcon>
                <p className={classes.cardCategory}>Followers</p>
                            <h3 className={classes.cardTitle}>{numberWorkflowsError}</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <Update />
                  Just Updated
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
                <h4 className={classes.cardTitle}>Daily Sales</h4>
                <p className={classes.cardCategory}>
                  <span className={classes.successText}>
                    <ArrowUpward className={classes.upArrowCardCategory} /> 55%
                  </span>{" "}
                  increase in today sales.
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
                <h4 className={classes.cardTitle}>Email Subscriptions</h4>
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
                        title="Queue"
                        headerColor="primary"
                        tabs={[
                            {
                                tabName: "Queue",
                                tabIcon: Code,
                                tabContent: (
                                    
                                            <Table
                                                tableHeaderColor="warning"
                                                tableHead={["ID", "Name", "Salary", "Country"]}
                                                tableData={[
                                                    ["1", "Dakota Rice", "$36,738", "Niger"],
                                                    ["2", "Minerva Hooper", "$23,789", "Curaçao"],
                                                    ["3", "Sage Rodriguez", "$56,142", "Netherlands"],
                                                    ["4", "Philip Chaney", "$38,735", "Korea, South"]
                                                ]}
                                            />
                                        
                                        )
                                    },
                            {
                                tabName: "Errors",
                                tabIcon: Code,
                                tabContent: (
                                            <Table
                                                tableHeaderColor="warning"
                                                tableHead={["ID", "Name", "Salary", "Country"]}
                                                tableData={[
                                                    ["1", "Dakota Rice", "$36,738", "Niger"],
                                                    ["2", "Minerva Hooper", "$23,789", "Curaçao"],
                                                    ["3", "Sage Rodriguez", "$56,142", "Netherlands"],
                                                    ["4", "Philip Chaney", "$38,735", "Korea, South"]
                                                ]}
                                            />
                                        )
                            },
                            {
                                tabName: "Processed",
                                tabIcon: Code,
                                tabContent: (
                                    <Table
                                        tableHeaderColor="warning"
                                        tableHead={["ID", "Name", "Salary", "Country"]}
                                        tableData={[
                                            ["1", "Dakota Rice", "$36,738", "Niger"],
                                            ["2", "Minerva Hooper", "$23,789", "Curaçao"],
                                            ["3", "Sage Rodriguez", "$56,142", "Netherlands"],
                                            ["4", "Philip Chaney", "$38,735", "Korea, South"]
                                        ]}
                                    />
                                )
                            }
                            ]}
                        />
                </GridItem>
                <GridItem xs={12} sm={12} md={6}>
                    <Card>
                        <CardHeader color="warning">
                            <h4 className={classes.cardTitleWhite}>Employees Stats</h4>
                            <p className={classes.cardCategoryWhite}>
                                New employees on 15th September, 2016
                </p>
                        </CardHeader>
                        <CardBody>
                            <Table
                                tableHeaderColor="warning"
                                tableHead={["ID", "Name", "Salary", "Country"]}
                                tableData={[
                                    ["1", "Dakota Rice", "$36,738", "Niger"],
                                    ["2", "Minerva Hooper", "$23,789", "Curaçao"],
                                    ["3", "Sage Rodriguez", "$56,142", "Netherlands"],
                                    ["4", "Philip Chaney", "$38,735", "Korea, South"]
                                ]}
                            />
                        </CardBody>
                    </Card>
                </GridItem>
        </GridContainer>
      </div>
    );
  }
}

JobCostPage.propTypes = {
  classes: PropTypes.object.isRequired
};

export default withStyles(dashboardStyle)(JobCostPage);
