Feature: Equipment
  The Equipment endpoints should provide basic CRUD operations for equipments

  Scenario: Get /api/Equipments returns 200
    Given Subquip api is running
    And I have a valid token
    When I send a GET request to "/api/Equipment/Equipments" with:
	| SortDirection | Page | PageSize |
        |         0     |  1   |   10     |
    Then the response status should be "200"   
    And the JSON response should have key "body"

  Scenario: Store equiment
    Given Subquip api is running
    And I have a valid token
    When I set JSON request body to:
      """
  {
	  "equipmentNumber": "string",
	  "serialNumber": "string",
	  "owner": "string",
	  "license": "string",
	  "location": "string",
	  "grossWeight": "string",
	  "sizeDimensions": "string"
	}
    	"""
    And I send a POST request to "/api/Equipment/Create"
    Then the response status should be "200"
    And the JSON response should have key "body"
    And the JSON response should have "$.body.equipmentId" as a non-empty string
  

  Scenario: Store and retrieve equiment
    Given Subquip api is running
    And I have a valid token
    When I set JSON request body to:
      """
  {
	  "equipmentNumber": "string",
	  "serialNumber": "string",
	  "owner": "string",
	  "license": "string",
	  "location": "string",
	  "grossWeight": "string",
	  "sizeDimensions": "string"
	}
	""" 
    And I send a POST request to "/api/Equipment/Create"
    And I grab "$.body.equipmentId" as "eqId"
    And I send a GET request to "/api/Equipment/Details" with:
        | id          |
        | {eqId}      |
    Then the response status should be "200"
    And the JSON response should have key "body"
    And the JSON response should have "$.body.equipmentId" as a non-empty string
    And the JSON response should have "$.body" with "equipmentId" of value "{eqId}"
