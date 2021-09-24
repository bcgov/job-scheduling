# job-scheduling
## Job Scheduling

This is a simple job scheduling job that runs on OpenShift as a cron job. It will make a call to a Dynamics API to request that the Dynamics environment do a task. This was created as a better alternative to scheduling within Dynamics.

## Schedule

This job calls Dynamics which, in turn, calls CAS to determine the status of any transactions previously sent but in an unknown status. As such, this job can be run once a day and is typically done early in the morning (say 7:00am). 

## Secrets

This job requires five secrets as follows:

* DYNAMICS_APP_GROUP_CLIENT_ID
* DYNAMICS_APP_GROUP_RESOURCE
* DYNAMICS_APP_GROUP_SECRET
* DYNAMICS_PASSWORD
* DYNAMICS_USERNAME
