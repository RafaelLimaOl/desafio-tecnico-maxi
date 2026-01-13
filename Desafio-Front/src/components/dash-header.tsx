"use client"

import { useAuth } from "@/hooks/useAuth"
import { LogOut } from "lucide-react"
import { usePathname } from "next/navigation"
import { ThemeToggle } from "./theme-toggle"
import { Button } from "./ui/button"
import { SidebarTrigger } from "./ui/sidebar"

const DashHeader = () => {
  const pathname = usePathname()

  const { logout } = useAuth()
  const pathSegments = pathname.split("/").filter(Boolean)

  const breadcrumbs = pathSegments.map((segment) =>
    segment.replace(/-/g, " ").replace(/\b\w/g, (l) => l.toUpperCase())
  )
  return (
    <nav className="p-4 w-full border-b-2">
      <div className="flex items-center justify-between gap-4">
        <div className="flex items-center">
          <SidebarTrigger />
          <h1 className="text-lg font-bold pl-2">{breadcrumbs.join(" > ")} </h1>
        </div>

        <div className="flex gap-2">
          <ThemeToggle />
          <Button onClick={logout}>
            <LogOut />
          </Button>
        </div>
      </div>
    </nav>
  )
}

export default DashHeader
