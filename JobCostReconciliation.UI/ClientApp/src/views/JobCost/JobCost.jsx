import React, { Component } from "react";
import ReactDOM from "react-dom";
import Pagination from "react-js-pagination";
import PropTypes from "prop-types";
// @material-ui/core
import withStyles from "@material-ui/core/styles/withStyles";
import Icon from "@material-ui/core/Icon";
// @material-ui/icons
import Store from "@material-ui/icons/Store";
import Warning from "@material-ui/icons/Warning";
import DateRange from "@material-ui/icons/DateRange";
import Update from "@material-ui/icons/Update";
import ArrowUpward from "@material-ui/icons/ArrowUpward";
import AccessTime from "@material-ui/icons/AccessTime";
import Accessibility from "@material-ui/icons/Accessibility";
import BugReport from "@material-ui/icons/BugReport";
import Code from "@material-ui/icons/Code";
import Queue from "@material-ui/icons/Queue";
// core components
import Button from 'components/CustomButtons/Button.jsx';
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
import 'jquery/src/jquery';
require("jquery");
require("bootstrap-less");

const styles = {
  cardCategoryWhite: {
    "&,& a,& a:hover,& a:focus": {
      color: "rgba(255,255,255,.62)",
      margin: "0",
      fontSize: "14px",
      marginTop: "0",
      marginBottom: "0"
    },
    "& a,& a:hover,& a:focus": {
      color: "#FFFFFF"
    }
  },
  cardTitleWhite: {
    color: "#FFFFFF",
    marginTop: "0px",
    minHeight: "auto",
    fontWeight: "300",
    fontFamily: "'Roboto', 'Helvetica', 'Arial', sans-serif",
    marginBottom: "3px",
    textDecoration: "none",
    "& small": {
      color: "#777",
      fontSize: "65%",
      fontWeight: "400",
      lineHeight: "1"
    }
  }
};

class JobCostPage extends React.Component {
  state = {
    value: 0,
    workflows: [],
    loadingWorkflows: true,
    workflowQueue: [],
    loadingWorkflowQueue: true,
    sentryEvents: [],
    loadingSentryEvents: true,
    workflowJobSummary: [], 
    loadingWorkflowJobSummary: true,
    jobData: [],
    loadingJobData: true,
    jobTotals: [],
    loadingJobTotals: true,
    activePage: 1
  };
  handleChange = (event, value) => {
    this.setState({ value });
  };

  handleChangeIndex = index => {
    this.setState({ value: index });
  };

  handlePageChange(pageNumber) {
    console.log(`active page is ${pageNumber}`);
    this.setState({ activePage: pageNumber });
  }

  componentDidMount() {
    if (this.state.workflows.length === 0) {
      this.getWorkflowData();
    }

    if (this.state.workflowQueue.length === 0) {
      this.getWorkflowQueueItems();
    }

    if (this.state.workflowJobSummary.length === 0) {
      this.getWorkflowJobSummary();
    }
  }

  getWorkflowData() {
    fetch('api/Workflow')
      .then(response => response.json())
      .then(data => {
        this.setState({ workflows: data, loadingWorkflows: false });
      });
  }

  getWorkflowQueueItems() {
      fetch('api/WorkflowQueue')
          .then(response => response.json())
          .then(data => {
              this.setState({ workflowQueue: data, loadingWorkflowQueue: false });
          });
  }

  getSentryEvents() {
    var url = 'https://sentry.io/api/0/projects/fischer-homes/sapphire-workflow-processor/events/?sentry_key=d6c4cc26a4a14211bde1206019bca81f';

    //var url = 'https://sentry.io/api/1444156/projects/fischer-homes/sapphire-workflow-processor/events/?sentry_key=d6c4cc26a4a14211bde1206019bca81f';

    var bearer = 'Bearer e2c2124126af4c28970c75a54f6975159adaf54e9971416ea2434292c53f1180';
    var dsn = 'DSN https://d6c4cc26a4a14211bde1206019bca81f@sentry.io/1444156';

    fetch(url, {
      method: 'GET',
      credentials: 'include',
      headers: {
        'access-control-allow-credentials': true,
        'authorization': bearer
      }      
    })
      .then(response => response)
      .then(data => {
        this.setState({ sentryEvents: Array.from(data), loadingSentryEvents: false });
      })
      .catch(error => { console.log(error) });
  }

  filterWorkflowQueue(workflowQueue, status) {

      return workflowQueue.filter((workflowQueue) =>
          status === workflowQueue.status);
  }

  getWorkflowJobSummary() {
    fetch('api/JobCost/WorkflowJobCostSummary')
      .then(response => response.json())
      .then(data => {
        this.setState({ workflowJobSummary: data, loadingWorkflowJobSummary: false });
      });
  }

  getJobSummaryByJob(jobNumber) {
    fetch('api/JobCost/JobTotals/'.concat(jobNumber))
      .then(response => response.json())
      .then(data => {
        this.setState({ jobTotals: data, loadingJobTotals: false });
      });
  }

  getJobSummaryByHomeRID(homeRID) {
    fetch('api/JobCost/JobData/'.concat(homeRID))
      .then(response => response.json())
      .then(data => {
        this.setState({ jobData: data, loadingJobData: false });
      });
  }

  renderSentryEventList(sentryEventList) {
    return (
      <div>
        {sentryEventList}
      </div>
      )
  }

  static renderWorkflowQueue(workflowQueue,  page_number = 1) {

    var page_size = 8;
    var queueData = workflowQueue.slice((page_number-1) * page_size, page_number * page_size);

    return (
        <tbody>
        {queueData.map(item =>
            <tr key={item.id}>
              <td>{item.id}</td>
            <td>{item.wFlowRID}</td>
            <td>{item.refObjRID}</td>
            <td>{item.status}</td>
          </tr>
          )}
        </tbody>
      )
  }

  static renderJobCostSummary(jobSummary, page_number = 1) {

    var page_size = 8;
    var jobSummaryData = jobSummary.slice((page_number - 1) * page_size, page_number * page_size);

    return (
      <tbody>
        {jobSummaryData.map(job =>
          <tr key={job.id}>
            <td>{job.jobNumber}</td>
            <td>{job.sapphireEgmTotal}</td>
            <td>{job.pervasiveEgmTotal}</td>
            <td></td>
          </tr>
        )}
      </tbody>
    )
  }
    

  render() {
    const { classes } = this.props;
    
    let numberWorkflows = this.state.loadingWorkflows ? "" : this.state.workflows.length;

    let queueData = this.state.loadingWorkflowQueue ? [] : JobCostPage.renderWorkflowQueue(this.state.workflowQueue);
    let numberWorkflowsProcessed = this.state.loadingWorkflowQueue ? "" : this.state.workflowQueue.length;

    let numberWorkflowsNew = this.state.loadingWorkflowQueue ? [] : this.filterWorkflowQueue(this.state.workflowQueue, "New").length;
    let numberWorkflowsError = this.state.loadingWorkflowQueue ? [] : this.filterWorkflowQueue(this.state.workflowQueue, "Error").length;
    let errorQueueData = this.state.loadingWorkflowQueue ? [] : JobCostPage.renderWorkflowQueue(this.filterWorkflowQueue(this.state.workflowQueue, "Error"));

    let workflowJobCostSummary = this.state.workflowJobSummary.length > 0 ? JobCostPage.renderJobCostSummary(this.state.workflowJobSummary) : [];

    let jobTotals = this.state.jobTotals.length > 0 ? JobCostPage.renderJobCostSummary(this.state.jobTotals) : [];

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
                <p className={classes.cardCategory}>Exceptions</p>
                <h3 className={classes.cardTitle}>5</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <Accessibility />
                  Tracked from Sentry.io
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
                <p className={classes.cardCategory}>Errors</p>
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
          <GridItem xs={12} sm={12} md={6}>
            <CustomTabs
              headerTitle="Workflow Queue"
              headerColor="primary"
              tabs={[
                {
                  tabName: "Queue",
                  tabIcon: Queue,
                  tabContent: (
                    <table width="100%">
                      <thead>
                        <tr>
                          <th align="left">Queue ID</th>
                          <th align="left">WorkflowRID</th>
                          <th align="left">RefObjRID</th>
                          <th align="left">Status</th>
                        </tr>
                      </thead>
                      {queueData}
                    </table>
                  )
                },
                {
                  tabName: "Errors",
                  tabIcon: Code,
                  tabContent: (
                    <table width="100%">
                      <thead>
                        <tr>
                          <th align="left">Queue ID</th>
                          <th align="left">WorkflowRID</th>
                          <th align="left">RefObjRID</th>
                          <th align="left">Status</th>
                        </tr>
                      </thead>
                      {errorQueueData}
                    </table>
                  )
                },
                {
                  tabName: "Processed",
                  tabIcon: Code,
                  tabContent: (
                    <CardBody>
                    <table width="100%">
                      <thead>
                        <tr>
                          <th align="left">JobNumber</th>
                          <th align="left">Sapphire Total</th>
                          <th align="left">Pervasive Total</th>
                        </tr>
                      </thead>
                      {workflowJobCostSummary}
                    </table>
                      <Pagination
                          activePage={this.state.activePage}
                          itemsCountPerPage={8}
                          totalItemsCount={this.state.workflowJobSummary.length}
                          pageRangeDisplayed={5}
                          onChange={this.handlePageChange}
                      />
                      </CardBody>
                  )
              }
              ]}
            />
          </GridItem>
          <GridItem xs={12} sm={12} md={6}>
            <Card>
              <CardHeader color="warning">
                <h4 className={classes.cardTitleWhite}>Job Cost Details</h4>
                <div>
                  <input ref="jobNumberInput" type="text" size="15" />
                    <Button
                      className="Button"
                      color="primary"
                      size="sm"
                      onClick={() => {
                        this.getJobSummaryByJob(this.refs.jobNumberInput.value);
                      }}>
                      Search
                    </Button>
                </div>
              <p className={classes.cardCategoryWhite}>
              </p>
                </CardHeader>
                <CardBody>
                <table width="100%">
                  <thead>
                    <tr>
                      <th align="left">JobNumber</th>
                      <th align="left">Sapphire Total</th>
                      <th align="left">Pervasive Total</th>
                    </tr>
                  </thead>
                  {jobTotals}
                </table>
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
