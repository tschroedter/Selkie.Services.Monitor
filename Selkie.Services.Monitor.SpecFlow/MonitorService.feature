Feature: MonitorService

Scenario: Ping Monitor Service
	Given Service is running
	And Did not receive ping response message
	When I send a ping message
	Then the result should be a ping response message

Scenario: Stop Monitor Service
	Given Service is running
	And Did not receive ping response message
	When I send a stop message
	Then the result should be service not running

Scenario: Stopping Monitor Service sends message
	Given Service is running
	When I send a stop message
	Then the result should be that I received a ServiceStoppedMessage

Scenario: Started Monitor Service sends message
	Given Service is running
	Then the result should be that I received a ServiceStartedMessage

Scenario: Status Request Monitor Service
	Given Service is running
	And Did not receive status response message
	When I send a status request message
	Then the result should be a status response message

Scenario: Monitor Service starts other Services
	Given Service is running
	And Did not receive ping response message for Lines Service
	And Did not receive ping response message for Racetracks Service
	Then the result should be that I received a ping response message for Lines Service
	Then the result should be that I received a ping response message for Racetracks Service

Scenario: Monitor Service restarts other Lines Services
	Given Service is running
	And I wait until services are running
	And I send a Lines Service stop message
	And Did not receive ping response message for Lines Service
	Then the result should be that I received a ping response message for Lines Service
