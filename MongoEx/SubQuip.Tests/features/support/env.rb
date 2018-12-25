require 'cucumber-api'
require 'rest-client'

RestClient::Request.class_eval do

  define_method(:normalize_url) do |url|
    prefix = ENV["URL_PREFIX"] || "localhost:3333"
    url = 'http://' + prefix + url unless url.match(%r{\A[a-z][a-z0-9+.-]*://}i)
    url
  end
end

AfterStep do |scenario|
  if @token then
    @headers = {} unless @headers
    @headers[:Authorization] = "Bearer #{@token}" unless @headers.key? :Authorization
    @headers[:'Content-Type'] = "application/json" unless @headers.key? :'Content-Type'
    @headers[:'Accept'] = "application/json" unless @headers.key? :'Accept'
  end
end
