require 'rest-client'


Given /^Subquip api is running$/ do
  if ENV["TEST_ENV"] != "CI" && !`docker ps`.include?("subquip-backend") then
  	# start subquip backend and wait for it to start
        puts `cd .. && docker-compose up -d`
        fail($?) unless $? == 0
        sleep(5)
  end
end

Given /^I have a valid token$/ do
  url = "/api/Login/LoginUser"
  headers = {
      :Accept => 'application/json',
      :'Content-Type' => 'application/json'
  }
  response = RestClient.post url, '{ "userName": "test", "userPassword": "test"  }', headers
  instance_variable_set(:@token, response.gsub(/^"|"$/, ''))
  @headers = { :Accept => 'application/json',
      :'Content-Type' => 'application/json',
      :Authorization => "Bearer #{response.gsub /^"|"$/, ''}"}
end

Then /^the JSON response should have "(.*?)" with "(.*?)" of value "(.*?)"$/ do |jpath, prop, value|
  
  value = instance_variable_get("@#{value.gsub(/^{|}$/, '')}") if value.start_with?("{") and value.end_with?("}")
  res = @response.get jpath
  if(Hash === res) then
    expect(res[prop].to_s).to eq value
  else
    expect(res.send(prop).to_s).to eq value
  end
end

Then /^the JSON response should have "(.*?)" of type "(.*?)" with "(.*?)" of value "(.*?)"$/ do |jpath, type, prop, value|
  value = instance_variable_get("@#{value.gsub(/^{|}$/, '')}") if value.start_with?("{") and value.end_with?("}")

  res = @response.get_as_type jpath, type
  if(Hash === res) then
    expect(res[prop].to_s).to eq value
  else
    expect(res.send(prop).to_s).to eq value
  end
end


Then /^the JSON response should have "(.*?)" as a non-empty string$/ do |jpath|
  res = @response.get_as_type jpath, "string"
  expect(res.strip()).not_to eq ""
end

