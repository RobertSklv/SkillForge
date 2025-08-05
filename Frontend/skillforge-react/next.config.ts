import type { NextConfig } from "next";

function getHostname() {
  return (process.env.NEXT_PUBLIC_BACKEND_DOMAIN as string).replace(/^(https?:\/\/)([^:/]+)(:\d+)?/, '$2');
}

const nextConfig: NextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: getHostname(),
      }
    ]
  }
};

export default nextConfig;
