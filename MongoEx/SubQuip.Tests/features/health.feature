Feature: Healthcheck endpoint
  Healthcheck enpoint should be useable as a helath and liveness check

  Scenario: Get /api/Health returns 200
    Given Subquip api is running
    When I send a GET request to "/api/Health"
    Then the response status should be "200"
