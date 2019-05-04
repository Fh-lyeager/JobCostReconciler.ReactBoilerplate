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
  },
  exceptionsTable: {
    fontSize: "12px",
    fontWeight: "300"
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

    if (this.state.sentryEvents.length === 0) {
      this.getSentryEvents();
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
    
    fetch('api/JobCost/sentry/events' )
      .then(response => response.json())
      .then(data => {
        this.setState({ sentryEvents: data, loadingSentryEvents: false });
      });
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

  static renderSentryEventList(sentryEventList) {
    return (
      <tbody>
        {sentryEventList.map(item =>
          <tr key={item.eventID}>
            <td>{item.title}</td>
          </tr>
        )}
      </tbody>
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
            <td onClick="">{job.jobNumber}</td>
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

    let sentryEventList = this.state.loadingSentryEvents ? [] : JobCostPage.renderSentryEventList(this.state.sentryEvents);

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
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 265.98 249.68">
                    <title>sentry-glyph-white</title>
                    <path d="M144.9,65.43a13.75,13.75,0,0,0-23.81,0l-19.6,33.95,5,2.87a96.14,96.14,0,0,1,47.83,77.4H140.56a82.4,82.4,0,0,0-41-65.54l-5-2.86L76.3,143l5,2.87a46.35,46.35,0,0,1,22.46,33.78H72.33a2.27,2.27,0,0,1-2-3.41l8.76-15.17a31.87,31.87,0,0,0-10-5.71L60.42,170.5a13.75,13.75,0,0,0,11.91,20.62h43.25v-5.73A57.16,57.16,0,0,0,91.84,139l6.88-11.92a70.93,70.93,0,0,1,30.56,58.26v5.74h36.65v-5.73A107.62,107.62,0,0,0,117.09,95.3L131,71.17a2.27,2.27,0,0,1,3.93,0l60.66,105.07a2.27,2.27,0,0,1-2,3.41H179.4c.18,3.83.2,7.66,0,11.48h14.24a13.75,13.75,0,0,0,11.91-20.62Z"
                  />
                  </svg>
                </CardIcon>
                <p className={classes.cardCategory}>Exceptions</p>
                <h3 className={classes.cardTitle}>5</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 300.98 300.68">
                    <title>sentry-glyph-black</title>
                    <path d="M144.9,65.43a13.75,13.75,0,0,0-23.81,0l-19.6,33.95,5,2.87a96.14,96.14,0,0,1,47.83,77.4H140.56a82.4,82.4,0,0,0-41-65.54l-5-2.86L76.3,143l5,2.87a46.35,46.35,0,0,1,22.46,33.78H72.33a2.27,2.27,0,0,1-2-3.41l8.76-15.17a31.87,31.87,0,0,0-10-5.71L60.42,170.5a13.75,13.75,0,0,0,11.91,20.62h43.25v-5.73A57.16,57.16,0,0,0,91.84,139l6.88-11.92a70.93,70.93,0,0,1,30.56,58.26v5.74h36.65v-5.73A107.62,107.62,0,0,0,117.09,95.3L131,71.17a2.27,2.27,0,0,1,3.93,0l60.66,105.07a2.27,2.27,0,0,1-2,3.41H179.4c.18,3.83.2,7.66,0,11.48h14.24a13.75,13.75,0,0,0,11.91-20.62Z"
                    />
                  </svg>
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
                  tabName: "Exceptions",
                  tabIcon: Code,
                  tabContent: (
                    <table width="100%" ref="exceptionsTable" id="exceptionsTable">
                      <thead>
                        <tr>
                          <th align="left">Title</th>
                        </tr>
                      </thead>
                      {sentryEventList}
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
